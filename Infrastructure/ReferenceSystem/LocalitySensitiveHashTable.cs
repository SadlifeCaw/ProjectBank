using System.Data;
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
        //public Dictionary<string, Bucket<Tagable>> Map;
        public IBucketRepository _bucketRepository;
        public IProjectRepository _projectRepository;


        public LocalitySensitiveHashTable(IBucketRepository repository, IProjectRepository projectRepository)
        {
            NumberOfGroups = k / groupSize;
            //Map = new Dictionary<string, Bucket<Tagable>>();
            _bucketRepository = repository;
            _projectRepository = projectRepository;
        }

        private async Task<(Response, BucketDTO)> AddSignature(string bucketString)
        {
           var dto = new BucketCreateDTO{ProjectIds = new HashSet<int>(),Key = bucketString};
           var task = await _bucketRepository.CreateAsync(dto);
           return task;
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
                if (bucketdto.ProjectIds.Contains(tagable.Id)) {
                    return Response.Conflict;
                    //throw new ArgumentException("Project already inserted");
                } else 
                {
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
            if(deleted == Response.Deleted && inserted == Response.Created) return Response.Updated;
            else return Response.Conflict;
        }

        public async Task<Response> Delete(Tagable tagable)
        {
            var bucketStrings = HashesToBucketString(tagable.Signature);
            foreach (string bucketString in bucketStrings)
            {
                var bucketdto = await _bucketRepository.ReadBucketByKeyAsync(bucketString);
                if(bucketdto != null && bucketdto.ProjectIds.Contains(tagable.Id)) await _bucketRepository.RemoveProjectAsync(bucketdto.Id, tagable.Id);
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
                var bucket = (await _bucketRepository.ReadBucketByKeyAsync(bucketString));
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
    }
}

