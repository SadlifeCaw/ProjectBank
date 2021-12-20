using System.Data;
using System.Text;
using System.Linq;
using ProjectBank.Infrastructure.Entities;

namespace ProjectBank.Infrastructure.ReferenceSystem
{
    public class LocalitySensitiveHashTable<Tagable> 
    where Tagable : ITagable
    {
        private int groupSize = 2;
        private int k = 6;
        public int NumberOfGroups;
        public Dictionary<string, Bucket<Tagable>> Map;
        //private ProjectBankContext _context;

        public LocalitySensitiveHashTable()//ProjectBankContext context)
        {
            NumberOfGroups = k / groupSize;
            Map = new Dictionary<string, Bucket<Tagable>>();
            //_context = context;
        }

        protected virtual void AddSignature(string bucketString)
        {
            //if(Map[bucketString].Projects == null) return Response.Conflict;
            Map[bucketString] = new Bucket<Tagable>();
        }

        public virtual async Task<Response> Insert(Tagable tagable)
        {
            if (tagable.Tags.Count == 0) { return Response.Conflict; }
            var bucketStrings = HashesToBucketString(tagable.Signature);
            await foreach (string bucketString in bucketStrings.ToAsyncEnumerable())
            {
                if (!Map.ContainsKey(bucketString)) AddSignature(bucketString);
                //!Map[bucketString].
                if (Map[bucketString].Projects.Where(x => x.Id == tagable.Id).ToList().Count != 0 || !Map[bucketString].Projects.Add(tagable)) { return Response.Conflict;; }
            }
            return Response.Created;
        }

        /*
           public async Task<Response> Insert(Tagable tagable)
        {
            if (tagable.Tags.Count() == 0) { return Response.Conflict; }
            var bucketStrings = HashesToBucketString(tagable.Signature);
            foreach (string bucketString in bucketStrings)
            {
                var bucketdto = await _bucketRepository.ReadBucketByKeyAsync(bucketString);
                if (bucketdto == null) bucketdto = (await AddSignature(bucketString)).Item2;
                if (bucketdto.ProjectIds.Contains(tagable.Id))
                {
                    return Response.Conflict;
                    //throw new ArgumentException("Project already inserted");
                }
                else
                {
                    Map[bucketString].ProjectIds.Add(tagable.Id);
                    var response = await _bucketRepository.AddProjectAsync(bucketdto.Id, tagable.Id);
                }
            }
            return Response.Created;
        }
        
        */

        public async Task<Response> Update(Tagable tagable)
        {
            var deleted = await Delete(tagable);
            if(deleted != Response.Deleted) return deleted;
            var inserted = await Insert(tagable);
            return inserted;
        }

        public async Task<Response> Delete(Tagable tagable)
        {
            //if()
            var bucketStrings = HashesToBucketString(tagable.Signature);
            await foreach (string bucketString in bucketStrings.ToAsyncEnumerable())
            {
                if(!Map.ContainsKey(bucketString)) return Response.NotFound;
                var toRemove = Map[bucketString].Projects.Where(x => x.Id == tagable.Id).ToList();
                if (!Map[bucketString].Projects.Remove(toRemove.FirstOrDefault())) return Response.NotFound;
            }
            return Response.Deleted;
        }


        public async Task<IEnumerable<Tagable>> Get(Tagable tagable)
        {
            HashSet<Tagable> set = new HashSet<Tagable>();
            var bucketStrings = HashesToBucketString(tagable.Signature);
            await foreach (string bucketString in bucketStrings.ToAsyncEnumerable())
            {
                foreach (Tagable relatedTagable in Map[bucketString].Projects)
                {
                    if (!relatedTagable.Id.Equals(tagable.Id))
                    {
                        set.Add(relatedTagable);
                    }
                }
            }
            return set.AsEnumerable();
        }
        /*
        public async Task<IEnumerable<int>> Get(Tagable tagable)
        {
            HashSet<int> set = new HashSet<int>();
            var bucketStrings = HashesToBucketString(tagable.Signature);
            foreach (string bucketString in bucketStrings)
            {
                //var bucket = (await _bucketRepository.ReadBucketByKeyAsync(bucketString));
                var bucket = Map[bucketString];
                foreach (int id in bucket.ProjectIds)
                {
                    if (id != tagable.Id)
                    {
                        set.Add(id);
                    }
                }
            }
            return set.AsEnumerable();
        }

        */

        public string[] HashesToBucketString(Signature signature)
        {

            string[] bucketStrings = new string[NumberOfGroups];
            for (int i = 0; i < NumberOfGroups; i++)
            {
                StringBuilder bucketStringBuilder = new StringBuilder();

                for (int j = 0; j < groupSize; j++)
                {
                    bucketStringBuilder.Append(signature.Hashes.ElementAt(i * groupSize + j));
                }

                bucketStrings[i] = bucketStringBuilder.ToString();
            }
            return bucketStrings;
        }
    }
}







/*using System.Data;
using System.Text;
using ProjectBank.Infrastructure.Entities;

namespace ProjectBank.Infrastructure.ReferenceSystem
{
    public class LocalitySensitiveHashTable<Tagable>
    where Tagable : ITagable
    {
        
        
        
        private int groupSize = 2;
        private int k = 6;
        public int NumberOfGroups;
        //public IBucketRepository _bucketRepository;
        //public IProjectRepository _projectRepository;
        //public ITagRepository _tagRepository;
        //public ITagRepository _tagRepository;

        public Dictionary<string, Bucket<Tagable>> Map;

        private ProjectBankContext _context;

        public LocalitySensitiveHashTable(ProjectBankContext context/*IBucketRepository repository, IProjectRepository projectRepository, ITagRepository tagRepository)
        {
            NumberOfGroups = k / groupSize;
            Map = new Dictionary<string, Bucket<Tagable>>();
            _context = context;
            //_bucketRepository = repository;
            //_projectRepository = projectRepository;
            //_tagRepository = tagRepository;
        }

        private void AddSignature(string bucketString)
        {
            //var dto = new BucketCreateDTO { ProjectIds = new HashSet<int>(), Key = bucketString };
            //var task = await _bucketRepository.CreateAsync(dto);
            
            Map[bucketString] = new Bucket<Tagable>();
            //return task;
            //Map[bucketString] = new Bucket<Tagable>();
        }

        public async Task<Response> Insert(Tagable tagable)
        {
            if (tagable.Tags.Count() == 0) { return Response.Conflict; }
            var bucketStrings = HashesToBucketString(tagable.Signature);
            foreach (string bucketString in bucketStrings)
            {
                var bucketdto = await _bucketRepository.ReadBucketByKeyAsync(bucketString);
                if (bucketdto == null) bucketdto = (await AddSignature(bucketString)).Item2;
                if (bucketdto.ProjectIds.Contains(tagable.Id))
                {
                    return Response.Conflict;
                    //throw new ArgumentException("Project already inserted");
                }
                else
                {
                    Map[bucketString].ProjectIds.Add(tagable.Id);
                    var response = await _bucketRepository.AddProjectAsync(bucketdto.Id, tagable.Id);
                }
            }
            return Response.Created;
        }

        public async Task<Response> Update(Tagable tagable)
        {
            var deleted = await Delete(tagable);
            //tagable.Tags = tags;
            var inserted = await Insert(tagable);
            if (deleted == Response.Deleted && inserted == Response.Created) return Response.Updated;
            else return Response.Conflict;
        }

        public async Task<Response> Delete(Tagable tagable)
        {
            var bucketStrings = HashesToBucketString(tagable.Signature);
            foreach (string bucketString in bucketStrings)
            {
                var bucketdto = await _bucketRepository.ReadBucketByKeyAsync(bucketString);
                if (bucketdto != null && bucketdto.ProjectIds.Contains(tagable.Id))
                {
                    await _bucketRepository.RemoveProjectAsync(bucketdto.Id, tagable.Id);
                    Map[bucketString].ProjectIds.Remove(tagable.Id);
                }
                else return Response.Conflict;
            }
            return Response.Deleted;
        }


        public async Task<IEnumerable<int>> Get(Tagable tagable)
        {
            HashSet<int> set = new HashSet<int>();
            var bucketStrings = HashesToBucketString(tagable.Signature);
            foreach (string bucketString in bucketStrings)
            {
                //var bucket = (await _bucketRepository.ReadBucketByKeyAsync(bucketString));
                var bucket = Map[bucketString];
                foreach (int id in bucket.ProjectIds)
                {
                    if (id != tagable.Id)
                    {
                        set.Add(id);
                    }
                }
            }
            return set.AsEnumerable();
        }

        public string[] HashesToBucketString(Signature signature)
        {

            string[] bucketStrings = new string[NumberOfGroups];
            for (int i = 0; i < NumberOfGroups; i++)
            {
                StringBuilder bucketStringBuilder = new StringBuilder();

                for (int j = 0; j < groupSize; j++)
                {
                    bucketStringBuilder.Append(signature.Hashes.ElementAt(i * groupSize + j));
                }

                bucketStrings[i] = bucketStringBuilder.ToString();
            }
            return bucketStrings;
        }

        public async Task<IEnumerable<int>> GetById(int i)
        {
            var projectdto = await _projectRepository.ReadByIDAsync(i);
            return await GetDTO(projectdto);
        }

        public async Task<IEnumerable<int>> GetById(int i, int size)
        {
            var projectdto = await _projectRepository.ReadByIDAsync(i);
            return (await GetDTO(projectdto)).Take(size);
        }

        private IReadOnlyCollection<Tag> dtoToTags(IEnumerable<TagDTO> dtos)
        {
            var tagList = new List<Tag>();
            foreach (var dto in dtos)
            {
                var tag = new Tag(dto.Name) { Id = dto.Id };
                tagList.Add(tag);
            }
            return tagList;
        }

        public async Task<IEnumerable<int>> GetDTO(ProjectDTO dto)
        {
            HashSet<int> set = new HashSet<int>();
            var tagsdtos = new List<TagDTO>();
            foreach (var id in dto.TagIDs)
            {
                tagsdtos.Add(await _tagRepository.ReadTagByIDAsync(id));
            }
            var tags = dtoToTags(tagsdtos);
            var signature = new Signature(tags);

            var bucketStrings = HashesToBucketString(signature);
            foreach (string bucketString in bucketStrings)
            {
                //var bucket = (await _bucketRepository.ReadBucketByKeyAsync(bucketString));
                var bucket = Map[bucketString];
                foreach (int id in bucket.ProjectIds)
                {
                    if (id != dto.Id)
                    {
                        set.Add(id);
                    }
                }
            }
            return set.AsEnumerable();
        }

        public async Task<Response> InsertAll(/*IEnumerable<Tagable> tagables)
        {
            var projects = await _projectRepository.ReadAllAsync();
            foreach (var dto in projects)
            {
                var project = toProject(dto);
                var response = await Insert(dto);
                //if(response != Response.Created) return Response.Conflict;
            }
            return Response.Updated;
        }

        private async Task<Project> toProject(ProjectDTO dto)
        {
            var project = new Project();
            project.Id = dto.Id;
            var tagsdtos = new List<TagDTO>();
            foreach (var id in dto.TagIDs)
            {
                tagsdtos.Add(await _tagRepository.ReadTagByIDAsync(id));
            }
            var tags = dtoToTags(tagsdtos);
            project.Tags = tags;
            //project.Buckets =  
            return project;
        }
//MODIFY THE WHOLE THING TO TAKE DTO INPUTS



        //FIND SOLUTION FOR CATEGORIZABLE and without casting
        /*public IReadOnlyCollection<Project> GetSortedInCategory(Project tagable)
        {
            var casted = tagable;
            if(casted.Category == null) throw new ArgumentException("The object provided does not have a category");
            var category = casted.Category;
            var allCategories = GetSorted(tagable);
            var sorted = new List<Project>();

            foreach (var tag in allCategories)
            {
                var project = (Project)tag;
                if(project.Category.IsRelated(category)) sorted.Add(project);
            }
            return sorted.AsReadOnly();
        }*/
   // }
//}

