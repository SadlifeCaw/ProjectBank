using System.Linq;
using System;
using System.Diagnostics;
using System.Text;
using Xunit;
using System.Collections.Generic;
using ProjectBank.Infrastructure;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure.ReferenceSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using ProjectBank.Core.EF.DTO;


namespace Performance.Tests
{
    public class LSHPerformanceTests
    {
        static int NumOfProjects = 10;
        static int NumOfTags = 50;
        static int NumOfTagsPerProject = 10;

        ProjectBankContext _context { get; set; }
        ProjectRepository _projectRepository { get; set; }
        BucketRepository _bucketRepository { get; set; }
        List<Tag> AllTags;
        List<Project> Projects;
        static Institution ITU = new Institution { Id = 1, Title = "ITU", Description = "ITU" };
        static Faculty FacultyComputerScienceITU = new Faculty { Id = 3, Institution = ITU, Title = "FC ITU", Description = "ITU" };
        static Supervisor Supervisor1 = new Supervisor("troe@itu.dk", ITU, "Troels", "Jyde", new List<Project>(), FacultyComputerScienceITU, new List<Project>());
        public LSHPerformanceTests()
        {
            AllTags = GenerateTags(NumOfTags).ToList();
            Projects = GenerateProjects(NumOfProjects).ToList();
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            //defer connection.CloseConnection();

            var builder = new DbContextOptionsBuilder<ProjectBankContext>();
            builder.UseSqlite(connection);
            //builder.UseSqlite(connection, x => x.SuppressForeignKeyEnforcement());
            //sqlBuilder.SuppressForeignKeyEnforcement();
            builder.EnableSensitiveDataLogging();

            var context = new ProjectBankContext(builder.Options);
            context.Database.EnsureCreated();
            context.SaveChanges();

            context.Categories.AddRange(ITU, FacultyComputerScienceITU);
            context.Users.Add(Supervisor1);
            foreach (var Tag in AllTags)
            {
                context.Tags.Add(Tag);
            }
            context.Projects.AddRange(Projects);
            context.SaveChanges();
            _context = context;
            _bucketRepository = new BucketRepository(_context);
            _projectRepository = new ProjectRepository(_context);
        }


        public void meh()
        {
            AllTags = GenerateTags(NumOfTags).ToList();
            Projects = GenerateProjects(NumOfProjects).ToList();
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            //defer connection.CloseConnection();

            var builder = new DbContextOptionsBuilder<ProjectBankContext>();
            builder.UseSqlite(connection);
            //builder.UseSqlite(connection, x => x.SuppressForeignKeyEnforcement());
            //sqlBuilder.SuppressForeignKeyEnforcement();
            builder.EnableSensitiveDataLogging();

            var context = new ProjectBankContext(builder.Options);
            context.Database.EnsureCreated();
            //context.Database.ExecuteSqlRaw("PRAGMA foreign_keys=OFF;");
            //context.Database.ExecuteSqlRaw("PRAGMA ignore_check_constraints=true;");
            foreach (var entity in context.Categories) context.Categories.Remove(entity);
            foreach (var entity in context.Users) context.Users.Remove(entity);
            foreach (var entity in context.Tags) context.Tags.Remove(entity);
            foreach (var entity in context.Projects) context.Projects.Remove(entity);
            context.SaveChanges();

            context.Categories.AddRange(ITU, FacultyComputerScienceITU);
            context.Users.Add(Supervisor1);
            //context.SaveChanges();
            //Tag blod = new Tag("Blab");
            foreach (var Tag in AllTags)
            {
                context.Tags.Add(Tag);
            }
            //context.Tags.Add(blod);
            //context.Tags.AddRange(AllTags);

            context.SaveChanges();
            //context.SaveChanges();

            context.Projects.AddRange(Projects);
            context.SaveChanges();

            _context = context;
            _bucketRepository = new BucketRepository(_context);
            _projectRepository = new ProjectRepository(_context);
        }
/*
        [Theory]
        [InlineData(2)]
        public async void Insert_performance(float seconds)
        {
           // ProjectLSH LSH = await GetInsertedLSH(Projects);
            meh();
            LocalitySensitiveHashTable<ITagable> LSHTest = new LocalitySensitiveHashTable<ITagable>(_bucketRepository, _projectRepository);
            var timer = Stopwatch.StartNew();
            foreach (ITagable tagable in Projects)
            {
                await LSHTest.Insert(tagable);
            }

            timer.Stop();

            Assert.True(TimeSpan.FromSeconds(seconds) >= timer.Elapsed, string.Format("Actual time was {0} milliseconds", timer.ElapsedMilliseconds));
        }
*/
        [Theory]
        [InlineData(0.01)]
        public async void Get_performance(float seconds)
        {
                        meh();

            ProjectLSH LSH = await GetInsertedLSH(Projects);

            Random random = new Random();
            Project tagable = Projects[random.Next(0, NumOfProjects - 1)];
            var timer = Stopwatch.StartNew();
            IEnumerable<int> relatedProjects = await LSH.Get(tagable);
            timer.Stop();
            Assert.True(TimeSpan.FromSeconds(seconds) >= timer.Elapsed, string.Format("Actual time was {0} milliseconds", timer.ElapsedMilliseconds));
        }

        [Theory]
        [InlineData(0.01)]
        public async void GetSorted_performance(float seconds)
        {
                        meh();

            ProjectLSH LSH = await GetInsertedLSH(Projects);

            Random random = new Random();
            Project tagable = Projects[random.Next(0, NumOfProjects - 1)];
            var timer = Stopwatch.StartNew();
            IEnumerable<ProjectDTO> relatedProjects = await LSH.GetSorted(tagable);
            timer.Stop();
            Assert.True(TimeSpan.FromSeconds(seconds) >= timer.Elapsed, string.Format("Actual time was {0} milliseconds", timer.ElapsedMilliseconds));
        }

        private async Task<ProjectLSH> GetInsertedLSH(List<Project> Projects)
        {
            ProjectLSH LSH = new ProjectLSH(_bucketRepository, _projectRepository, _context);
            foreach (Project tagable in Projects)
            {
                await LSH.Insert(tagable);
            }
            return LSH;
        }


        private IEnumerable<Tag> GenerateTags(int NumOfTags)
        {
            if (NumOfTags > 17576) throw new ArgumentException("Too many tags, the maximum allowed is 17576"); //26^3 = 17576
            //Generate tags
            char limit = 'z';
            int id = 1;
            char[] characters = new char[] { 'a', 'a', 'a' };
            HashSet<Tag> tags = new HashSet<Tag>();
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
                var tag = new Tag(name);
                tag.Id = id;
                tag.Projects = new List<Project>();
                tags.Add(tag);
                //yield return new Tag(name);
            }
            return tags;
            //yield break;
        }
        public IEnumerable<Project> GenerateProjects(int NumOfProjects)
        {
            if (NumOfTags < NumOfTagsPerProject) throw new Exception("NumOfTags cannot be lower than NumOfTagsPerProject");
            Random random = new Random();
            int id = 1;

            for (int i = 0; i < NumOfProjects; i++, id++)
            {
                List<Tag> projectTags = new List<Tag>();
                for (int j = 0; j < NumOfTagsPerProject; j++)
                {
                    bool tagInserted = false;
                    while (!tagInserted)
                    {
                        var tag = AllTags[random.Next(0, AllTags.Count() - 1)];
                        if (projectTags.Contains(tag)) continue;
                        projectTags.Add(tag);
                        tagInserted = true;
                    }

                }

                StringBuilder projectName = new StringBuilder();
                foreach (Tag tag in projectTags)
                {
                    projectName.Append(tag.Name);
                }
                projectName.Append(i);

                // new Project { Category = ITU, Tags = new List<Tag> { Agriculture, Food }, Id = 6, Author = Supervisor1, Title = "AgricultureFoodIdentical", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>() , MaxStudents = 5};
                //Category
                //Author (supervisor)
                //
                var project = new Project() { MaxStudents = 5, Id = id, Author = Supervisor1, Title = projectName.ToString(), Description = projectName.ToString(), Status = ProjectBank.Core.ProjectStatus.PUBLIC, Category = ITU, Tags = projectTags, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>() };

                /*foreach(Tag tag in projectTags)
                {
                    tag.Projects.Add(project);
                }*/

                yield return project;
            }
            yield break;
        }

    }

}

