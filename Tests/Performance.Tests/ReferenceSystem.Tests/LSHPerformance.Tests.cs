using System.Linq;
using System;
using System.Diagnostics;
using System.Text;
using Xunit;
using System.Collections.Generic;
using ProjectBank.Infrastructure;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure.ReferenceSystem;
namespace Performance.Tests
{
    public class LSHPerformanceTests
    {
        static int NumOfProjects = 10000;
        static int NumOfTags = 500;
        static int NumOfTagsPerProject = 10;
        //static List<Tag> AllTags = GenerateTags(NumOfTags).ToList();
        //static List<ITagable> Projects = GenerateProjects(NumOfProjects, AllTags).ToList();
        //LocalitySensitiveHashTable<ITagable> LSH = GetInsertedLSH(Projects);
        public LSHPerformanceTests()
        {

        }
        /*
        [Theory]
        [InlineData(2)]
        public void Insert_performance(float seconds)
        {
            LocalitySensitiveHashTable<ITagable> LSHTest = new LocalitySensitiveHashTable<ITagable>();
            var timer = Stopwatch.StartNew();
            foreach (ITagable tagable in Projects)
            {
                LSHTest.Insert(tagable);
            }

            timer.Stop();

            Assert.True(TimeSpan.FromSeconds(seconds) >= timer.Elapsed, string.Format("Actual time was {0} milliseconds", timer.ElapsedMilliseconds));
        }

        [Theory]
        [InlineData(0.01)]
        public void Get_performance(float seconds)
        {
            Random random = new Random();
            ITagable tagable = Projects[random.Next(0, NumOfProjects-1)];
            var timer = Stopwatch.StartNew();
            List<ITagable> relatedProjects = LSH.Get(tagable).ToList();
            timer.Stop();
            Assert.True(TimeSpan.FromSeconds(seconds) >= timer.Elapsed, string.Format("Actual time was {0} milliseconds", timer.ElapsedMilliseconds));
        }

        [Theory]
        [InlineData(0.01)]
        public void GetSorted_performance(float seconds)
        {
            Random random = new Random();
            ITagable tagable = Projects[random.Next(0, NumOfProjects-1)];
            var timer = Stopwatch.StartNew();
            List<ITagable> relatedProjects = LSH.GetSorted(tagable).ToList();
            timer.Stop();
            Assert.True(TimeSpan.FromSeconds(seconds) >= timer.Elapsed, string.Format("Actual time was {0} milliseconds", timer.ElapsedMilliseconds));
        }

        private static LocalitySensitiveHashTable<ITagable> GetInsertedLSH(List<ITagable> Projects){
            LocalitySensitiveHashTable<ITagable> LSH = new LocalitySensitiveHashTable<ITagable>();
            foreach(ITagable tagable in Projects){
                LSH.Insert(tagable);
            }
            return LSH;
        }


        private static IEnumerable<Tag> GenerateTags(int NumOfTags)
        {
            if (NumOfTags > 17576) throw new ArgumentException("Too many tags, the maximum allowed is 17576"); //26^3 = 17576
            //Generate tags
            char limit = 'z';
            int id = 0;
            char[] characters = new char[] { 'a', 'a', 'a' };
            for (int i = 0; i < NumOfTags; i++, id++)
            {
                string name = new String(characters);

                characters[2]++;
                if (characters[2] > limit)
                {
                    characters[2] = 'a';
                    characters[1]++;
                    if (characters[1] > limit)
                    {
                        characters[1] = 'a';
                        characters[0]++;
                    }
                }

                yield return new Tag(name);
            }
            yield break;
        }
        public static IEnumerable<ITagable> GenerateProjects(int NumOfProjects, List<Tag> AllTags)
        {
            Random random = new Random();
            int id = 0;
            for (int i = 0; i < NumOfProjects; i++, id++)
            {
                List<Tag> projectTags = new List<Tag>();
                for (int j = 0; j < NumOfTagsPerProject; j++)
                {
                    projectTags.Add(AllTags[random.Next(0, AllTags.Count()-1)]);
                }

                StringBuilder projectName = new StringBuilder();
                foreach (Tag tag in projectTags)
                {
                    projectName.Append(tag.Name);
                }
                yield return new Project() { Id = id, Author = null, Title = projectName.ToString(), Description = projectName.ToString(), Status = ProjectBank.Core.ProjectStatus.PUBLIC, Category = null, Tags = projectTags };
            }
            yield break;
        }
        */
    }
}

