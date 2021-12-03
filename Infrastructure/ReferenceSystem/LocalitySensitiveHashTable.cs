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
        public Dictionary<string, Bucket> Map;

        public LocalitySensitiveHashTable()
        {
            NumberOfGroups = k / groupSize;
            Map = new Dictionary<string, Bucket>();
        }

        private void AddSignature(string bucketString)
        {
            Map[bucketString] = new Bucket();
        }

        public void Insert(Tagable tagable)
        {
            if (tagable.Tags.Count() == 0) { throw new ArgumentException("Cannot insert project without tags"); }
            var bucketStrings = HashesToBucketString(tagable.Signature);
            foreach (string bucketString in bucketStrings)
            {
                if (!Map.ContainsKey(bucketString)) AddSignature(bucketString);
                if (!Map[bucketString].Projects.Add(tagable)) { throw new ArgumentException("Project already inserted"); }
            }
        }

        public void Update(Tagable tagable)
        {
            Delete(tagable);
            //tagable.Tags = tags;
            Insert(tagable);
        }

        public void Delete(Tagable tagable)
        {
            //if()
            var bucketStrings = HashesToBucketString(tagable.Signature);
            foreach (string bucketString in bucketStrings)
            {
                if (!Map.ContainsKey(bucketString) || !Map[bucketString].Projects.Remove(tagable)) throw new ArgumentException("Project does not exist in the table.");
            }
        }


        public IEnumerable<Tagable> Get(Tagable tagable)
        {
            HashSet<Tagable> set = new HashSet<Tagable>();
            var bucketStrings = HashesToBucketString(tagable.Signature);
            foreach (string bucketString in bucketStrings)
            {
                foreach (Tagable relatedTagable in Map[bucketString].Projects)
                {
                    if (!relatedTagable.Equals(tagable))
                    {
                        set.Add(relatedTagable);
                    }
                }
            }
            return set.AsEnumerable();
        }

        public IReadOnlyCollection<Tagable> GetSorted(Tagable tagable)
        {
            var NotSortedTagables = Get(tagable);

            List<(float, Tagable)> jaccardList = new List<(float, Tagable)>();

            foreach (Tagable project in NotSortedTagables)
            {
                var tags = new HashSet<Tag>(project.Tags);
                int inCommon = 0;

                foreach (Tag tag in tagable.Tags)
                {
                    if (tags.Contains(tag))
                    {
                        inCommon++;
                    }
                }

                int total = project.Tags.Count + tagable.Tags.Count - inCommon;
                float jaccardindex = inCommon / (float)total;

                jaccardList.Add((jaccardindex, project));
            }
            jaccardList.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            List<Tagable> SortedTagables = new List<Tagable>();
            foreach ((float, Tagable) tagger in jaccardList)
            {
                SortedTagables.Add(tagger.Item2);
            }
            return SortedTagables.AsReadOnly();
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

