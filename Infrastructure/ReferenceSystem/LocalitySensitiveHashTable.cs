using System.Data;
using System.Text;
using ProjectBank.Infrastructure.Entities;

namespace ProjectBank.Infrastructure.ReferenceSystem
{
    public class LocalitySensitiveHashTable
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

        public void Insert(ITagable tagable)
        {
            if (tagable.Tags.Count() == 0) { throw new ArgumentException("Cannot insert project without tags"); }
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
                if (!Map.ContainsKey(bucketString) || !Map[bucketString].Projects.Remove(tagable)) throw new ArgumentException("Project does not exist in the table.");
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

        //FIND SOLUTION FOR CATEGORIZABLE
        public IReadOnlyCollection<Project> GetSortedInCategory(ITagable tagable, Category category)
        {
            var allCategories = GetSorted(tagable);
            var categories = new List<Project>();

            foreach (var tag in allCategories)
            {
                var project = (Project)tag;
                if (Match(project, category))
                {
                    categories.Add(project);
                }
            }
            return categories.AsReadOnly();
        }

        private bool Match(Project project, Category category)
        {
            var match = false;
            if (project.Category.GetType() == typeof(Institution))
            {
                match = MatchParentInstitution((Institution)project.Category, category);
            }
            else if (project.Category.GetType() == typeof(Faculty))
            {
                match = MatchParentFaculty((Faculty)project.Category, category);
            }
            else if (project.Category.GetType() == typeof(Program))
            {
                match = MatchParentProgram((Program)project.Category, category);
            }
            else if (project.Category.GetType() == typeof(Course))
            {
                match = MatchParentCourse((Course)project.Category, category);
            }
            return match;
        }


        private bool MatchParentInstitution(Institution course, Category category)
        {
            var match = false;
            if (course == category)
            {
                match = true;
            }
            return match;
        }
        private bool MatchParentFaculty(Faculty course, Category category)
        {

            var match = false;
            if (course == category || course.Institution == category)
            {
                match = true;
            }
            return match;
        }
        private bool MatchParentProgram(Program course, Category category)
        {
            var match = false;
            if (course == category || course.Faculty == category || course.Faculty.Institution == category)
            {
                match = true;
            }
            return match;
        }

        private bool MatchParentCourse(Course course, Category category)
        {
            var match = false;
            if (course == category)
            {
                match = true;
            }
            else
            {
                var programs = course.Programs;
                foreach (var program in programs)
                {
                    if (program == category || program.Faculty == category || program.Faculty.Institution == category)
                    {
                        match = true;
                    }
                }
            }
            return match;
        }
    }
}

