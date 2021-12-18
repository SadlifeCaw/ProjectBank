using ProjectBank.Infrastructure.Entities;

namespace ProjectBank.Infrastructure.ReferenceSystem
{
    public class ProjectLSH : LocalitySensitiveHashTable<IProject>
    {
        private ProjectBankContext _context;
        public ProjectLSH(IBucketRepository repository, IProjectRepository projectRepository, ProjectBankContext context) : base(repository, projectRepository)
        {
            _context = context;
        }
        public async Task<IReadOnlyCollection<ProjectDTO>> GetSortedInCategory(IProject tagable)
        {
            var casted = tagable;
            if (casted.Category == null) throw new ArgumentException("The object provided does not have a category");
            var category = casted.Category;
            var allCategories = await GetSorted(tagable);
            var sorted = new List<ProjectDTO>();

            foreach (var tag in allCategories)
            {
                var project = tag;
                var innerCategory = _context.Categories
                        .Where(c => c.Id == project.CategoryID)
                        .Select(c => c)
                        .FirstOrDefaultAsync();

                if (innerCategory.Result.IsRelated(category)) sorted.Add(project);
            }
            return sorted.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<ProjectDTO>> GetSorted(IProject tagable)
        {
            var NotSortedTagables = await Get(tagable);

            var jaccardList = new List<(float, ProjectDTO)>();
            var tagIds = new List<int>();
            foreach (var tag in tagable.Tags) tagIds.Add(tag.Id);
            foreach (int project in NotSortedTagables)
            {
                var tags = new HashSet<int>((await _projectRepository.ReadByIDAsync(project)).Value.TagIDs);
                int inCommon = 0;
                foreach (int tagid in tagIds)
                {
                    if (tags.Contains(tagid))
                    {
                        inCommon++;
                    }
                }

                int total = tags.Count + tagIds.Count - inCommon;
                float jaccardindex = inCommon / (float)total;

                jaccardList.Add((jaccardindex, await _projectRepository.ReadByIDAsync(project)));
            }
            jaccardList.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            var SortedTagables = new List<ProjectDTO>();
            foreach ((float, ProjectDTO) tagger in jaccardList)
            {
                SortedTagables.Add(tagger.Item2);
            }
            return SortedTagables.AsReadOnly();
        }
    }
}
