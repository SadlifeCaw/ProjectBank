using ProjectBank.Infrastructure.Entities;

namespace ProjectBank.Infrastructure.ReferenceSystem
{
    public class ProjectLSH : LocalitySensitiveHashTable<IProject>, IProjectLSH
    {
        private ProjectRepository _projectRepository;
        private ITagRepository _tagRepository;
        private ICategoryRepository _categoryRepository;

        public ProjectLSH(IProjectRepository projectRepository, ITagRepository tagRepository, ICategoryRepository categoryRepository)
        {
            _projectRepository = (ProjectRepository) projectRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
        }
        public override async Task<Response> Insert(IProject project)
        {
            if (project.Signature == null || project.Category == null) return Response.Conflict;
            return await base.Insert(project);           
        }

        public async Task<IReadOnlyCollection<IProject>> GetSortedInCategory(IProject tagable)
        {
            var category = tagable.Category;
            var allCategories = await GetSorted(tagable);
            var sorted = new List<IProject>();

            await foreach (var tag in allCategories.ToAsyncEnumerable())
            {
                if (tag.Category.Id == category.Id) sorted.Add(tag);
            }
            return sorted.AsReadOnly();
        }

        public async Task<Response> InsertAll()
        {
            //var dtos = (await _projectRepository.ReadAllAsync());
            var allProjects = await _projectRepository.ReadAllProjectReferenceAsync();//AllProjectReference(dtos);//await _projectRepository.ReadAllProjectReferenceAsync();
            Console.WriteLine("DONE");
            foreach (var project in allProjects)
            {
                var response = await Insert(project);
                if (response != Response.Created) return Response.Conflict;
            }
            return Response.Created;
        }

/*        private async IAsyncEnumerable<IProject> AllProjectReference(IReadOnlyCollection<ProjectDTO> dtos)
        {
            var projects = new List<IProject>();
            
            await foreach (var dto in dtos.ToAsyncEnumerable())
            {
                var tags = new List<Tag>();
                foreach (var name in dto.TagNames)
                {
                    tags.Add(new Tag(name));
                }
                var category = new Category{Id = dto.CategoryID};
                yield return (new ProjectReference() { Id = dto.Id, Tags = tags, Category = category, Signature = new Signature(tags)});
            }
        }
*/
        public async Task<IReadOnlyCollection<IProject>> GetSorted(IProject tagable)
        {
            var NotSortedTagables = await Get(tagable);

            var jaccardList = new List<(float, ITagable)>();
            var tagNames = new List<string>();
            foreach(var tag in tagable.Tags) tagNames.Add(tag.Name);
            foreach (var project in NotSortedTagables)
            {
                var tags = project.Tags;
                int inCommon = 0;
                foreach (var tag in tags)
                {
                    if (tagNames.Contains(tag.Name))
                    {
                        inCommon++;
                    }
                }

                int total = tags.Count + tagable.Tags.Count - inCommon;
                float jaccardindex = inCommon / (float)total;

                jaccardList.Add((jaccardindex, project));
            }
            jaccardList.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            var SortedTagables = new List<IProject>();
            foreach ((float, IProject) tagger in jaccardList)
            {
                SortedTagables.Add(tagger.Item2);
            }
            return SortedTagables.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<ProjectReferenceDTO>> GetSorted(IProject project, int size)
        {
            
            var sorted = (await GetSorted(project)).Take(size);
            var limited = new List<ProjectReferenceDTO>();

            foreach (var p in sorted) limited.Add(new ProjectReferenceDTO(p.Id, p.Category.Id,
                                                     p.Tags.Select(t => t.Id).ToList()));
            return limited.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<ProjectReferenceDTO>> GetSortedInCategory(IProject project, int size)
        {
            var sorted = (await GetSortedInCategory(project)).Take(size);
            var limited = new List<ProjectReferenceDTO>();

            foreach (var p in sorted) limited.Add(new ProjectReferenceDTO(p.Id, p.Category.Id,
                                                     p.Tags.Select(t => t.Id).ToList()));
            return limited.AsReadOnly(); 
        }

        /*private async Task<IProject> getProjectById(int id)
        {
            var dto = (await _projectRepository.ReadByIDAsync(id)).Value;
            var tags = new List<Tag>();
            foreach (var tagid in dto.TagIDs)
            {
                var tagdto = (await _tagRepository.ReadTagByIDAsync(tagid)).Value;
                tags.Add(new Tag(tagdto.Name));
            }
            var categoryDTO = (await _categoryRepository.Read(dto.CategoryID));
            return (new ProjectReference() { Id = dto.Id, Tags = tags, Category = new Category(){Id = categoryDTO.Id, Description = categoryDTO.Description, Title = categoryDTO.Title}, Signature = new Signature(tags) });
        }*/

    }

}
