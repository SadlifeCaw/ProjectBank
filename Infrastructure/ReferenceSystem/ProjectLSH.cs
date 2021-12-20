using ProjectBank.Infrastructure.Entities;

namespace ProjectBank.Infrastructure.ReferenceSystem
{
    public class ProjectLSH : LocalitySensitiveHashTable<IProject>, IProjectLSH
    {
        //private readonly ProjectBankContext _context;
        //private IBucketRepository _bucketRepository;
        private IProjectRepository _projectRepository;
        private ITagRepository _tagRepository;
        private ICategoryRepository _categoryRepository;


        public ProjectLSH(IProjectRepository projectRepository, ITagRepository tagRepository, ICategoryRepository categoryRepository)
        {
            //_context = context;
            //_bucketRepository = new BucketRepository(_context);
            _projectRepository = projectRepository;
            _tagRepository = tagRepository;
            _categoryRepository = categoryRepository;
        }

        protected override void AddSignature(string bucketString)
        {
            base.AddSignature(bucketString);
            //_context.Buckets.Add(new ProjectBucket(new HashSet<Project>(), bucketString));
            //_context.SaveChanges();
            //var dto = new BucketCreateDTO { ProjectIds = new HashSet<int>(), Key = bucketString };
            //var task = await _bucketRepository.CreateAsync(dto);
            //return task.Item1;
        }

        public override async Task<Response> Insert(IProject project)
        {
            if (project.Signature == null || project.Category == null) return Response.Conflict;
            return await base.Insert(project);
            /*var bucketinserted = await AddBucketAsync(HashesToBucketString(project.Signature), project);
            if (inserted == Response.Created && bucketinserted == Response.Created) return inserted;
            else if (inserted == Response.Created) return bucketinserted;
            else return Response.Conflict;
            //Change PROJECTBUCKET   */
        }

        /* private async Task<Response> AddBucketAsync(String[] groups, IProject project)
         {
             await foreach (var s in groups.ToAsyncEnumerable())
             {
                 var bucket = await _bucketRepository.ReadBucketByKeyAsync(s);//_context.Buckets.Where(b => b.Key == s).Select(b => b).FirstOrDefaultAsync();
                 if (bucket == null) return Response.NotFound;
                 //_context.Buckets.Remove(bucket);
                 //_context.Buckets.
                 var response = await _bucketRepository.AddProjectAsync(bucket.Id, project.Id);
                 if(response != Response.Updated) return Response.Conflict;
                 //_context.Buckets.Add(bucket);
             }
             //await _context.SaveChangesAsync();
             return Response.Created;
         }*/


        public async Task<IReadOnlyCollection<IProject>> GetSortedInCategory(IProject tagable)
        {
           //if (tagable.Category == null) return (null, Response.Conflict);
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
            var allProjectsDTO = await _projectRepository.ReadAllAsync();
            var allProjects = toProjects(allProjectsDTO);
            //foreach(var dto in allProjectsDTO) Console.WriteLine(dto.TagIDs.Count()); 
            //foreach(var bucket in _context.Buckets) Console.WriteLine(bucket.Projects.Count());
            await foreach (var project in allProjects)
            {
                /*Console.WriteLine(project.Id);
                Console.WriteLine(project.Title);
                Console.WriteLine(project.MaxStudents);
                Console.WriteLine(project.Description);
                Console.WriteLine("Is tags null: " + project.Tags == null);
                if(project.Tags != null) Console.WriteLine(project.Tags.Count()); //+ " COunt: " + project.Tags.Count());
                Console.WriteLine("Is users null: " + project.Users == null);
                Console.WriteLine("Is buckets null: " + project.Buckets == null);*/
                var response = await Insert(project);
                if (response != Response.Created) return Response.Conflict;
            }
            return Response.Created;
        }

        private async IAsyncEnumerable<IProject> toProjects(IReadOnlyCollection<ProjectDTO> dtos)
        {
            //var allTags = await _context.Tags;
            var tagMap = new Dictionary<int, Tag>();
            foreach (var tag in await _tagRepository.ReadAllAsync()) tagMap.Add(tag.Id, new Tag(tag.Name));
            var categoryMap = new Dictionary<int, Category>();
            foreach (var category in await _categoryRepository.Read())
            {
                 categoryMap.Add(category.Id, new Category(){Id = category.Id, Description = category.Description, Title = category.Title});
            }
            var projects = new List<IProject>();
            await foreach (var dto in dtos.ToAsyncEnumerable())
            {
                var tags = new List<Tag>();
                //await foreach(var tag in dto.TagIDs.ToAsyncEnumerable()) tags.Add(await _context.Tags.Where(c => c.Id == tag).Select(c => c).FirstOrDefaultAsync());
                foreach (var id in dto.TagIDs)
                {
                    tags.Add(tagMap[id]);
                }
                //await foreach(var category in dto.CategoryID.ToAsyncEnumerable()) tags.Add(await _context.Tags.Where(c => c.Id == tag).Select(c => c).FirstOrDefaultAsync());
                yield return (new ProjectReference() { Id = dto.Id, Tags = tags, Category = categoryMap[dto.CategoryID], Signature = new Signature(tags) });
                //projects.Add(new Project { Category = await _context.Categories.Where(c => c.Id == dto.CategoryID).Select(c => c).FirstOrDefaultAsync(), Tags = tags, Id = dto.Id, Author = null, Title = "" + dto.Id, Description = "", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>(), MaxStudents = 5 });   
            }
            //return projects;
        }

        public async Task<IReadOnlyCollection<IProject>> GetSorted(IProject tagable)
        {
            var NotSortedTagables = await Get(tagable);

            var jaccardList = new List<(float, ITagable)>();
            var tagNames = new List<string>();
            foreach(var tag in tagable.Tags) tagNames.Add(tag.Name);
            foreach (var project in NotSortedTagables)
            {
                var tags = project.Tags;//new HashSet<int>((await _projectRepository.ReadByIDAsync(project)).Value.TagIDs);
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
            
            //var project = await getProjectById(id);
            var sorted = (await GetSorted(project)).Take(size);
            var limited = new List<ProjectReferenceDTO>();

            foreach (var p in sorted) limited.Add(new ProjectReferenceDTO(p.Id, p.Category.Id,
                                                     p.Tags.Select(t => t.Id).ToList()));
            return limited.AsReadOnly();
            //var limited = new List<ProjectDTO>();

            //for(int p = 0; p < size; p++) {limited.Add(new ProjectDTO(p.Id, p.Author.Id, p.Title, p.Description, p.Status, p.MaxStudents, p.Category.Id,p.Tags.Select(t => t.Id).ToList(), p.Users.Select(u => u.Id).ToList(), p.Buckets.Select(b => b.Id).ToList()))}

        }

        public async Task<IReadOnlyCollection<ProjectReferenceDTO>> GetSortedInCategory(IProject project, int size)
        {
            var sorted = (await GetSortedInCategory(project)).Take(size);
            var limited = new List<ProjectReferenceDTO>();

            foreach (var p in sorted) limited.Add(new ProjectReferenceDTO(p.Id, p.Category.Id,
                                                     p.Tags.Select(t => t.Id).ToList()));
            return limited.AsReadOnly(); 
        }

        private async Task<IProject> getProjectById(int id)
        {
            var dto = (await _projectRepository.ReadByIDAsync(id)).Value;
            var tags = new List<Tag>();
            //await foreach(var tag in dto.TagIDs.ToAsyncEnumerable()) tags.Add(await _context.Tags.Where(c => c.Id == tag).Select(c => c).FirstOrDefaultAsync());
            foreach (var tagid in dto.TagIDs)
            {
                var tagdto = (await _tagRepository.ReadTagByIDAsync(tagid)).Value;
                tags.Add(new Tag(tagdto.Name));
                //tags.Add(await _context.Tags.Where(t => t.Id == tagid).Select(t => t).FirstOrDefaultAsync());
            }
            //await foreach(var category in dto.CategoryID.ToAsyncEnumerable()) tags.Add(await _context.Tags.Where(c => c.Id == tag).Select(c => c).FirstOrDefaultAsync());
            var categoryDTO = (await _categoryRepository.Read(dto.CategoryID));
            return (new ProjectReference() { Id = dto.Id, Tags = tags, Category = new Category(){Id = categoryDTO.Id, Description = categoryDTO.Description, Title = categoryDTO.Title}, Signature = new Signature(tags) });

            /*return await (_context.Projects
                        .Where(p => p.Id == id)
                        .Select(p => p))
                        .FirstOrDefaultAsync();*/
        }

    }

}
