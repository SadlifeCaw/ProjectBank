using System.Data;
using System.Text;
using ProjectBank.Infrastructure.Entities;

namespace ProjectBank.Infrastructure.ReferenceSystem
{
    public class LocalitySensitiveHashTable
    {
        private int groupSize = 2;
        private int k = 6;
        private int NumberOfGroups;
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

        public void Insert(ITagable tagable)
        {
            var bucketStrings = HashesToBucketString(tagable.Signature);
            foreach (string bucketString in bucketStrings)
            {
                if (!Map.ContainsKey(bucketString)) AddSignature(bucketString);
                if (!Map[bucketString].Projects.Add(tagable)) { throw new ArgumentException("Project already inserted"); }
            }
        }

        public void Update(ITagable tagable)
        {
            Delete(tagable);
            //tagable.Tags = tags;
            Insert(tagable);
        }

        public void Delete(ITagable tagable)
        {
            //if()
            var bucketStrings = HashesToBucketString(tagable.Signature);
            foreach (string bucketString in bucketStrings)
            {
                if(!Map.ContainsKey(bucketString) || !Map[bucketString].Projects.Remove(tagable)) throw new ArgumentException("Project does not exist in the table.");  
            }
        }


        public IEnumerable<ITagable> Get(ITagable tagable)
        {
            HashSet<ITagable> set = new HashSet<ITagable>();
            var bucketStrings = HashesToBucketString(tagable.Signature);
            foreach (string bucketString in bucketStrings)
            {
                foreach (ITagable relatedTagable in Map[bucketString].Projects)
                {
                    if (!relatedTagable.Equals(tagable))
                    {
                        set.Add(relatedTagable);
                    }
                }
            }
            return set.AsEnumerable();
        }

        public IReadOnlyCollection<ITagable> GetSorted(ITagable tagable)
        {
            var NotSortedTagables = Get(tagable);

            List<(float, ITagable)> jaccardList = new List<(float, ITagable)>();

            foreach (ITagable project in NotSortedTagables)
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
            List<ITagable> SortedTagables = new List<ITagable>();
            foreach ((float, ITagable) tagger in jaccardList)
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
    }
}

