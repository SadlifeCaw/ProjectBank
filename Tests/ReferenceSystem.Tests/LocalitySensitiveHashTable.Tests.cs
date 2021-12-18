using System.Runtime.CompilerServices;
using System;
using System.Linq;
using System.Text;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using ProjectBank.Infrastructure;
using ProjectBank.Core.EF.Repository;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure.ReferenceSystem;
using ProjectBank.Core.EF.DTO;
using ProjectBank.Core;
using System.Threading.Tasks;  
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;


namespace ReferenceSystem.Tests
{
    public class ProjectLSHTests
    {
        private readonly ProjectBankContext _context;
        private readonly IProjectRepository _projectRepository;
        private readonly IBucketRepository _bucketRepository;

        //Tests:
        //
        //HashestobucketString splits up a signature correctly
        //Insert throws exception for project with no tags
        //Insert works for projects with a lot of tags
        //Delete and update won't work, if project doesn't exist
        //Test insert into empty map

        Tag Agriculture = new Tag("Agriculture");
        Tag ComputerScience = new Tag("Computer Science");
        Tag Security = new Tag("Security");
        Tag Algorithms = new Tag("Algorithms");
        Tag Simulation = new Tag("Simulation");
        Tag Food = new Tag("Food");
        Tag Farming = new Tag("Farming");

        Project AgricultureFood;
        Project ComputerScienceSimulationAlgorithmsAgriculture;
        Project ComputerScienceAlgorithmsSecurity;
        Project AgricultureFarming;
        Project ComputerScienceAlgorithmsSimulationSecurity;

        //ProjectLSH LSH;
        ProjectLSH LargeLSH;

        IList<ProjectLSH> LSHList;
        public ProjectLSHTests()
        {
            //SQL lite
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var builder = new DbContextOptionsBuilder<ProjectBankContext>();
            builder.UseSqlite(connection);
            builder.EnableSensitiveDataLogging();

            var context = new ProjectBankContext(builder.Options);
            context.Database.EnsureCreated();

            context.Categories.Add(ITU);
            context.Categories.Add(DTU);
            context.Categories.Add(FacultyComputerScienceITU);
            context.Categories.Add(FacultyDesignDTU);
            context.Categories.Add(DataScience);
            context.Categories.Add(InteractiveDesign);
            context.Categories.Add(UIDesign);
            context.Categories.Add(IntroductoryProgramming);
            context.Users.Add(Villum);
            context.Users.Add(Anton);
            context.Users.Add(Supervisor1);
            //await context.SaveChanges();

            context.Tags.Add(Agriculture);
            context.Tags.Add(ComputerScience);
            context.Tags.Add(Algorithms);
            context.Tags.Add(Security);
            context.Tags.Add(Simulation);
            context.Tags.Add(Food);
            context.Tags.Add(Farming);
            context.Tags.Add(Mathematics);
            context.Tags.Add(Algebra);
            context.Tags.Add(DiscreteMathematics);
            context.Tags.Add(Calculus);
            context.Tags.Add(Statistics);
            context.Tags.Add(Probability);
            context.Tags.Add(Science);

            context.SaveChanges();


            //SMALL LSH:
            AgricultureFood = new Project { Category = ITU, Tags = new List<Tag> { Agriculture, Food }, Id = 1, Author = Supervisor1, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>(), MaxStudents = 5};
            ComputerScienceSimulationAlgorithmsAgriculture = new Project { Category = ITU, Tags = new List<Tag> { ComputerScience, Simulation, Algorithms, Agriculture }, Id = 2, Author = Supervisor1, Title = "ComputerScienceSimulationAlgorithms", Description = "ComputerScienceSimulationAlgorithmsAgriculture", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>() , MaxStudents = 5};
            ComputerScienceAlgorithmsSecurity = new Project { Category = ITU, Tags = new List<Tag> { ComputerScience, Algorithms, Security }, Id = 3, Author = Supervisor1, Title = "ComputerScienceAlgorithmsSecurity", Description = "ComputerScienceAlgorithmsSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>() , MaxStudents = 5};
            AgricultureFarming = new Project { Category = UIDesign, Tags = new List<Tag> { Agriculture, Farming, Food }, Id = 4, Author = Supervisor1, Title = "AgricultureFarming", Description = "AgricultureFarming", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>() , MaxStudents = 5};
            ComputerScienceAlgorithmsSimulationSecurity = new Project { Category = InteractiveDesign, Tags = new List<Tag> { ComputerScience, Algorithms, Simulation, Security }, Id = 5, Author = Supervisor1, Title = "ComputerScienceAlgorithmsSimulationSecurity", Description = "ComputerScienceAlgorithmsSimulationSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>() , MaxStudents = 5};

            //LARGE LSH:
            AgricultureFoodIdentical = new Project { Category = ITU, Tags = new List<Tag> { Agriculture, Food }, Id = 6, Author = Supervisor1, Title = "AgricultureFoodIdentical", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>() , MaxStudents = 5};
            ComputerScienceSimulationAlgorithmsAgricultureIdentical = new Project { Category = FacultyComputerScienceITU, Tags = new List<Tag> { ComputerScience, Simulation, Algorithms, Agriculture }, Id = 7, Author = Supervisor1, Title = "ComputerScienceSimulationAlgorithmsIdentical", Description = "ComputerScienceSimulationAlgorithmsAgriculture", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>() , MaxStudents = 5};
            ComputerScienceAlgorithmsSecurityIdentical = new Project { Category = IntroductoryProgramming, Tags = new List<Tag> { ComputerScience, Algorithms, Security }, Id = 8, Author = Supervisor1, Title = "ComputerScienceAlgorithmsSecurityIdentical", Description = "ComputerScienceAlgorithmsSecurityIdentical", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>() , MaxStudents = 5};
            AgricultureFarmingIdentical = new Project { Category = UIDesign, Tags = new List<Tag> { Agriculture, Farming, Food }, Id = 9, Author = Supervisor1, Title = "AgricultureFarming", Description = "AgricultureFarmingIdentical", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>() , MaxStudents = 5};
            ComputerScienceAlgorithmsSimulationSecurityIdentical = new Project { Category = IntroductoryProgramming, Tags = new List<Tag> { ComputerScience, Algorithms, Simulation, Security }, Id = 10, Author = Supervisor1, Title = "ComputerScienceAlgorithmsSimulationSecurityIdentical", Description = "ComputerScienceAlgorithmsSimulationSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>() , MaxStudents = 5};

            FoodProject = new Project { Category = IntroductoryProgramming, Tags = new List<Tag> { Food }, Id = 11, Author = Supervisor1, Title = "Title", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            FoodAndAgricultureProject = new Project { Category = InteractiveDesign, Tags = new List<Tag> { Food, Agriculture }, Id = 12, Author = Supervisor1, Title = "Title2", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            AgricultureSecurityProject = new Project { Category = DataScience, Tags = new List<Tag> { Agriculture, Security }, Id = 13, Author = Supervisor1, Title = "Title3", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            SecurityFoodProject = new Project { Category = FacultyComputerScienceITU, Tags = new List<Tag> { Food, Security }, Id = 14, Author = Supervisor1, Title = "Title4", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            SecurityProject = new Project { Category = FacultyDesignDTU, Tags = new List<Tag> { Security }, Id = 15, Author = Supervisor1, Title = "Title5", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};

            ManyTagsProject = new Project { Category = ITU, Tags = new List<Tag> { Agriculture, ComputerScience, Security, Algorithms, Simulation, Food, Farming }, Id = 16, Author = Supervisor1, Title = "Title6", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            LessManyTagsProject = new Project { Category = ITU, Tags = new List<Tag> { Agriculture, ComputerScience, Security, Simulation, Food, Farming }, Id = 17, Author = Supervisor1, Title = "AgricultureFood7", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            LessManyTagsProject2 = new Project { Category = DTU, Tags = new List<Tag> { ComputerScience, Security, Simulation, Food, Farming, Algorithms }, Id = 18, Author = Supervisor1, Title = "AgricultureFood8", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            LessManyTagsProject3 = new Project { Category = DTU, Tags = new List<Tag> { ComputerScience, Security, Simulation, Food, Algorithms, Agriculture }, Id = 19, Author = Supervisor1, Title = "AgricultureFood9", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};

            AllUnrelatedTags = new Project { Tags = new List<Tag> { Mathematics, Algebra, DiscreteMathematics, Calculus, Statistics, Probability, Science }, Id = 20, Author = Supervisor1, Title = "AgricultureFood10", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Category = ITU, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            MathAlgebraDiscrete = new Project { Tags = new List<Tag> { Mathematics, Algebra, DiscreteMathematics }, Id = 27, Author = Supervisor1, Title = "AgricultureFood11", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Category = DTU, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            ProbabilityProject = new Project { Tags = new List<Tag> { Probability }, Id = 21, Author = Supervisor1, Title = "AgricultureFood12", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Category = FacultyComputerScienceITU, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            CalcStatProbScience = new Project { Tags = new List<Tag> { Calculus, Statistics, Probability, Science }, Id = 22, Author = Supervisor1, Title = "AgricultureFood13", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Category = FacultyDesignDTU, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            AlgDiscCalcStat = new Project { Tags = new List<Tag> { Algebra, DiscreteMathematics, Calculus, Statistics }, Id = 23, Author = Supervisor1, Title = "AgricultureFood14", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Category = DataScience, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            MathAlgDisc = new Project { Tags = new List<Tag> { Mathematics, Algebra, DiscreteMathematics }, Id = 24, Author = Supervisor1, Title = "AgricultureFood15", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Category = InteractiveDesign, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            MathAlgDiscCalc = new Project { Tags = new List<Tag> { Mathematics, Algebra, DiscreteMathematics, Calculus }, Id = 25, Author = Supervisor1, Title = "AgricultureFood16", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Category = IntroductoryProgramming, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};
            CalcStatProbScience2 = new Project { Tags = new List<Tag> { Calculus, Statistics, Probability, Science }, Id = 26, Author = Supervisor1, Title = "AgricultureFood17", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Category = UIDesign, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>()  , MaxStudents = 5};

            context.Projects.Add(AgricultureFood);
            context.Projects.Add(ComputerScienceSimulationAlgorithmsAgriculture);
            context.Projects.Add(ComputerScienceAlgorithmsSecurity);
            context.Projects.Add(AgricultureFarming);
            context.Projects.Add(ComputerScienceAlgorithmsSimulationSecurity);

            context.Projects.Add(AgricultureFoodIdentical);
            context.Projects.Add(ComputerScienceSimulationAlgorithmsAgricultureIdentical);
            context.Projects.Add(ComputerScienceAlgorithmsSecurityIdentical);
            context.Projects.Add(AgricultureFoodIdentical);
            context.Projects.Add(ComputerScienceAlgorithmsSimulationSecurityIdentical);

            context.Projects.Add(FoodProject);
            context.Projects.Add(FoodAndAgricultureProject);
            context.Projects.Add(AgricultureSecurityProject);
            context.Projects.Add(SecurityFoodProject);
            context.Projects.Add(SecurityProject);

            context.Projects.Add(ManyTagsProject);
            context.Projects.Add(LessManyTagsProject);
            context.Projects.Add(LessManyTagsProject2);
            context.Projects.Add(LessManyTagsProject3);

            context.Projects.Add(AllUnrelatedTags);
            context.Projects.Add(MathAlgebraDiscrete);
            context.Projects.Add(ProbabilityProject);
            context.Projects.Add(CalcStatProbScience);
            context.Projects.Add(AlgDiscCalcStat);
            context.Projects.Add(MathAlgDisc);
            context.Projects.Add(MathAlgDiscCalc);
            context.Projects.Add(CalcStatProbScience2);

            context.SaveChanges();
            _context = context;
            _projectRepository = new ProjectRepository(_context);
            _bucketRepository = new BucketRepository(_context);

            /*LSH = new ProjectLSH(_bucketRepository, _projectRepository, _context);
            
            LSH.Insert(AgricultureFarming);
            LSH.Insert(ComputerScienceSimulationAlgorithmsAgriculture);
            LSH.Insert(ComputerScienceAlgorithmsSecurity);
            LSH.Insert(AgricultureFood);
            LSH.Insert(ComputerScienceAlgorithmsSimulationSecurity);*/

            LargeLSH = new ProjectLSH(_bucketRepository, _projectRepository, _context);

            LSHList = new List<ProjectLSH>();
            //LSHList.Add(LSH); //Index 0
            LSHList.Add(LargeLSH); //Index 1
        }

        private async Task<Response> inserts()
        {
            await LargeLSH.Insert(AgricultureFarmingIdentical);
            await LargeLSH.Insert(ComputerScienceSimulationAlgorithmsAgricultureIdentical);
            await LargeLSH.Insert(ComputerScienceAlgorithmsSecurityIdentical);
            await LargeLSH.Insert(AgricultureFoodIdentical);
            await LargeLSH.Insert(ComputerScienceAlgorithmsSimulationSecurityIdentical);
            await LargeLSH.Insert(AgricultureFarming);
            await LargeLSH.Insert(ComputerScienceSimulationAlgorithmsAgriculture);
            await LargeLSH.Insert(ComputerScienceAlgorithmsSecurity);
            await LargeLSH.Insert(AgricultureFood);
            await LargeLSH.Insert(ComputerScienceAlgorithmsSimulationSecurity);
            await LargeLSH.Insert(FoodProject);
            await LargeLSH.Insert(FoodAndAgricultureProject);
            await LargeLSH.Insert(AgricultureSecurityProject);
            await LargeLSH.Insert(SecurityFoodProject);
            await LargeLSH.Insert(SecurityProject);
            await LargeLSH.Insert(ManyTagsProject);
            await LargeLSH.Insert(LessManyTagsProject);
            await LargeLSH.Insert(LessManyTagsProject2);
            await LargeLSH.Insert(LessManyTagsProject3);
            await LargeLSH.Insert(AllUnrelatedTags);
            await LargeLSH.Insert(MathAlgebraDiscrete);
            await LargeLSH.Insert(ProbabilityProject);
            await LargeLSH.Insert(CalcStatProbScience);
            await LargeLSH.Insert(AlgDiscCalcStat);
            await LargeLSH.Insert(MathAlgDisc);
            await LargeLSH.Insert(MathAlgDiscCalc);
            await LargeLSH.Insert(CalcStatProbScience2);
            return Response.Created;
        }

        [Fact]
        public async void testInsert()
        {
            var list = new List<Response>();
            list.Add(await LargeLSH.Insert(AgricultureFarmingIdentical));
            list.Add(await LargeLSH.Insert(ComputerScienceSimulationAlgorithmsAgricultureIdentical));
            list.Add(await LargeLSH.Insert(ComputerScienceAlgorithmsSecurityIdentical));
            list.Add(await LargeLSH.Insert(AgricultureFoodIdentical));
            list.Add(await LargeLSH.Insert(ComputerScienceAlgorithmsSimulationSecurityIdentical));
            list.Add(await LargeLSH.Insert(AgricultureFarming));
            list.Add(await LargeLSH.Insert(ComputerScienceSimulationAlgorithmsAgriculture));
            list.Add(await LargeLSH.Insert(ComputerScienceAlgorithmsSecurity));
            list.Add(await LargeLSH.Insert(AgricultureFood));
            list.Add(await LargeLSH.Insert(ComputerScienceAlgorithmsSimulationSecurity));
            list.Add(await LargeLSH.Insert(FoodProject));
            list.Add(await LargeLSH.Insert(FoodAndAgricultureProject));
            list.Add(await LargeLSH.Insert(AgricultureSecurityProject));
            list.Add(await LargeLSH.Insert(SecurityFoodProject));
            list.Add(await LargeLSH.Insert(SecurityProject));
            list.Add(await LargeLSH.Insert(ManyTagsProject));
            list.Add(await LargeLSH.Insert(LessManyTagsProject));
            list.Add(await LargeLSH.Insert(LessManyTagsProject2));
            list.Add(await LargeLSH.Insert(LessManyTagsProject3));
            list.Add(await LargeLSH.Insert(AllUnrelatedTags));
            list.Add(await LargeLSH.Insert(MathAlgebraDiscrete));
            list.Add(await LargeLSH.Insert(ProbabilityProject));
            list.Add(await LargeLSH.Insert(CalcStatProbScience));
            list.Add(await LargeLSH.Insert(AlgDiscCalcStat));
            list.Add(await LargeLSH.Insert(MathAlgDisc));
            list.Add(await LargeLSH.Insert(MathAlgDiscCalc));
            list.Add(await LargeLSH.Insert(CalcStatProbScience2));
            foreach(var test in list)
            {
                Assert.Equal(test, Response.Created);
            }
        }

        private IEnumerable<Tag> Generate1000Tags()
        {
            //Generate tags
            char limit = 'z';
            int id = 0;
            char[] characters = new char[] { 'a', 'a', 'a' };
            for (int i = 0; i < 1000; i++, id++)
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

        public IEnumerable<IProject> Generate100000Projects(List<Tag> tags)
        {
            Random random = new Random();
            int tagsPerProject = 6;
            int id = 0;
            for (int i = 0; i < 100000; i++, id++)
            {
                List<Tag> projectTags = new List<Tag>();
                for (int j = 0; j < tagsPerProject; j++)
                {
                    projectTags.Add(tags[random.Next(0, 999)]);
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


       [Fact]
        public async void Update_Replaces_Project_With_Updated()
        {
            //Arrange
            //var tagable = AgricultureFarming;
            AgricultureFarming.Tags.Append(Food);
            IEnumerable<Tag> expectedTags = new List<Tag> { Agriculture, Farming, Food }.AsEnumerable();

            //Act
            await LargeLSH.Update(AgricultureFarming);

            IEnumerable<Tag> actualTags = new List<Tag> { };

            foreach (string buckestString in LargeLSH.HashesToBucketString(AgricultureFarming.Signature))
            {
                var bucket = await _context.Buckets.Where(b => b.Key == buckestString).Select(b => b).FirstOrDefaultAsync();
                foreach (IProject tagableInBucket in bucket.Projects)
                {
                    if (AgricultureFarming == tagableInBucket)
                    {
                        actualTags = tagableInBucket.Tags.AsEnumerable();
                        break;
                    }
                }
            }

            //Assert
            foreach (string buckestString in LargeLSH.HashesToBucketString(AgricultureFarming.Signature))
            {
                Assert.True((await _context.Buckets.Where(b => b.Key == buckestString).Select(b => b).FirstOrDefaultAsync()).Projects.Contains(AgricultureFarming));
            }

            Assert.Equal(expectedTags, actualTags);
        }

        //When calling Get, all projects have at least two simular tags
        //Only works for k = 6
        /*[Fact]
        public void SMALL_Get_Returns_Tagables_With_One_Or_More_Common_Tags()
        {
            //Arrange
            var Singature = ComputerScienceAlgorithmsSecurity.Signature;
            var buckets = new List<string>();
            StringBuilder builder = new StringBuilder();
            builder.Append(Singature.Hashes.ElementAt(0));
            builder.Append(Singature.Hashes.ElementAt(1));
            buckets.Add(builder.ToString());
            builder.Clear();
            builder.Append(Singature.Hashes.ElementAt(2));
            builder.Append(Singature.Hashes.ElementAt(3));
            buckets.Add(builder.ToString());
            builder.Clear();
            builder.Append(Singature.Hashes.ElementAt(4));
            builder.Append(Singature.Hashes.ElementAt(5));
            buckets.Add(builder.ToString());

            foreach (var str in buckets)
            {
                var bucketdto = _bucketRepository.ReadBucketByKeyAsync(str);
                var bucket = _context.Buckets.ElementAt(bucketdto.Result.Id);
                //var bucket = LSH.Map[str];
                foreach (IProject project in bucket.Projects)
                {
                    int counter = 0;
                    var tags = project.Tags;
                    foreach (var tag in tags)
                    {
                        counter++;
                    }
                    Assert.True(counter > 0);
                }
            }
        }
*/
        [Fact]
        public async void Get_Returns_Tagables_With_One_Or_More_Common_Tags()
        {
            await inserts();
            //Arrange
            var Singature = ComputerScienceAlgorithmsSecurity.Signature;
            var buckets = new List<string>();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                builder.Append(Singature.Hashes.ElementAt(i * 2));
                builder.Append(Singature.Hashes.ElementAt(i * 2 + 1));
                buckets.Add(builder.ToString());
                builder.Clear();
            }

            //Act & Assert
            foreach (var str in buckets)
            {
                var bucketdto = await _bucketRepository.ReadBucketByKeyAsync(str);
                var bucket = await _context.Buckets.Where(b => b.Id == bucketdto.Id).Select(id => id).FirstOrDefaultAsync();
                foreach (IProject project in bucket.Projects)
                {
                    int counter = 0;
                    var tags = project.Tags;
                    foreach (var tag in tags)
                    {
                        counter++;
                    }
                    Assert.True(counter > 0);
                }
            }
        }

        [Fact]
        public async void Projects_Within_Bucket_Has_One_Or_More_Common_Tags()
        {
            await inserts();
            string bucketString = LargeLSH.HashesToBucketString(ComputerScienceAlgorithmsSecurity.Signature)[0];
            var bucketdto = await _bucketRepository.ReadBucketByKeyAsync(bucketString);
            var bucket = await _context.Buckets.Where(b => b.Id == bucketdto.Id).Select(b => b).FirstOrDefaultAsync();
            //Bucket<IProject> bucket = LargeLSH.Map[bucketString];

            foreach (IProject tagable in bucket.Projects)
            {
                int CommonCounter = 0;
                foreach (Tag tag in ComputerScienceAlgorithmsSecurity.Tags)
                {
                    if (tagable.Tags.Contains(tag)) CommonCounter++;
                }
                Assert.True(CommonCounter > 0);
            }
        }

        //New bucket is made if signature group does not exist


        //TAKE THIS BACK
        [Fact]
        public async void Insert_Adds_BucketGroup_If_Non_Existant()
        {
            //Arrange
            //_context.Users.;
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var builder = new DbContextOptionsBuilder<ProjectBankContext>();
            builder.UseSqlite(connection);
            builder.EnableSensitiveDataLogging();

            var context = new ProjectBankContext(builder.Options);
            context.Database.EnsureCreated();

            var projectRepo = new ProjectRepository(context);
            var bucketRepo = new BucketRepository(context);
    
            var TestLSH = new ProjectLSH(bucketRepo, projectRepo, context);
            
            var AgriFarmSignature = new Signature(AgricultureFarming.Tags);
            var AgriFoodSignature = new Signature(AgricultureFood.Tags);

            //Act
            await TestLSH.Insert(AgricultureFarming);
            var bucketSizeBeforeInsert = (await bucketRepo.ReadAllAsync()).Count();
            await TestLSH.Insert(AgricultureFood);
            var bucketSizeAfterInsert = (await bucketRepo.ReadAllAsync()).Count();
            int notCommon = 0;
            for (int i = 0; i < AgriFarmSignature.Hashes.Count(); i++)
            {
                if (!AgriFarmSignature.Hashes.ElementAt(i).Equals(AgriFoodSignature.Hashes.ElementAt(i)))
                {
                    notCommon++;
                }
            }
            //Assert
            var expected = bucketSizeBeforeInsert + notCommon;
            var actual = bucketSizeAfterInsert;
            Assert.Equal(expected, actual);
        }
/*
        [Fact]
        public void SMALL_GetSorted_returns_similar_projects_sorted_by_highest_jaccardindex()
        {
            //Arrange
            var expected = new List<IProject> { ComputerScienceAlgorithmsSimulationSecurity, ComputerScienceSimulationAlgorithmsAgriculture }.AsEnumerable();

            //Act
            var actual = LSH.GetSorted(ComputerScienceAlgorithmsSecurity).AsEnumerable();

            //Assert
            Assert.Equal(expected, actual);
        }*/
        [Fact]
        public async void groupCountThreeEquals()
        {
            await inserts();
            var buckets = await _bucketRepository.ReadAllAsync();
            var counter = 0;
            var strings = LargeLSH.HashesToBucketString(ComputerScienceAlgorithmsSecurity.Signature);

            foreach(var b in buckets)
            {
                foreach(var s in strings)
                if(b.Key == s) 
                {
                    counter++;
                }
            }
            Assert.Equal(counter, 3);

            
            //_projectRepository.
            //_bucketRepository.
            //Assert.Equal(new Signature(ComputerScienceAlgorithmsSecurity.Tags).Hashes.ToString(), ComputerScienceAlgorithmsSecurity.Signature.Hashes.ToString());
        }

        [Fact]
        public async void signatureCount()
        {
            await inserts();
            //var buckets = await _bucketRepository.ReadAllAsync();
            //var counter = 0;
            var strings = LargeLSH.HashesToBucketString(ComputerScienceAlgorithmsSecurity.Signature);
            var strings2 = LargeLSH.HashesToBucketString(ComputerScienceAlgorithmsSimulationSecurity.Signature);
            //var strings2 = LargeLSH.HashesToBucketString(ComputerScienceAlgorithmsSecurityIdentical.Signature);
            var bucket1 = await _bucketRepository.ReadBucketByKeyAsync(strings[0]);
            var bucket2 = await _bucketRepository.ReadBucketByKeyAsync(strings[1]);
            var bucket3 = await _bucketRepository.ReadBucketByKeyAsync(strings[2]);
            var bucket11 = await _bucketRepository.ReadBucketByKeyAsync(strings2[0]);
            var bucket22 = await _bucketRepository.ReadBucketByKeyAsync(strings2[1]);
            var bucket33 = await _bucketRepository.ReadBucketByKeyAsync(strings2[2]);
            Assert.Equal(bucket1.Id, bucket1.Id);
            Assert.Equal(bucket2.Id, bucket22.Id);
            Assert.Equal(bucket3.Id, bucket33.Id);

/*
            foreach(var b in buckets)
            {
                foreach(var s in strings)
                if(b.Key == s) 
                {
                    counter++;
                }
            }
            */

            //Assert.Equal(strings, strings2);

            //_projectRepository.
            //_bucketRepository.
            //Assert.Equal(new Signature(ComputerScienceAlgorithmsSecurity.Tags).Hashes.ToString(), ComputerScienceAlgorithmsSecurity.Signature.Hashes.ToString());
        }

        [Fact]
        public async void LARGE_GetSorted_returns_similar_projects_sorted_by_highest_jaccardindex()
        {
            await inserts();
            //Arrange
            var ComputerScienceAlgorithmsSecurityIdenticalDTO = await _projectRepository.ReadByIDAsync(ComputerScienceAlgorithmsSecurityIdentical.Id);
            var ComputerScienceAlgorithmsSimulationSecurityIdenticalDTO = await _projectRepository.ReadByIDAsync(ComputerScienceAlgorithmsSimulationSecurityIdentical.Id);
            var ComputerScienceAlgorithmsSimulationSecurityDTO = await _projectRepository.ReadByIDAsync(ComputerScienceAlgorithmsSimulationSecurity.Id);
            var LessManyTagsProject2DTO = await _projectRepository.ReadByIDAsync(LessManyTagsProject2.Id);
            var ComputerScienceSimulationAlgorithmsAgricultureIdenticalDTO = await _projectRepository.ReadByIDAsync(ComputerScienceSimulationAlgorithmsAgricultureIdentical.Id);
            var ComputerScienceSimulationAlgorithmsAgricultureDTO = await _projectRepository.ReadByIDAsync(ComputerScienceSimulationAlgorithmsAgriculture.Id);            
            var expected = new List<int> { ComputerScienceAlgorithmsSecurityIdenticalDTO.Value.Id, ComputerScienceAlgorithmsSimulationSecurityIdenticalDTO.Value.Id, ComputerScienceAlgorithmsSimulationSecurityDTO.Value.Id, LessManyTagsProject2DTO.Value.Id, ComputerScienceSimulationAlgorithmsAgricultureIdenticalDTO.Value.Id, ComputerScienceSimulationAlgorithmsAgricultureDTO.Value.Id}.AsEnumerable();
            
            //Act
            var actualDTO = (await LargeLSH.GetSorted(ComputerScienceAlgorithmsSecurity)).AsEnumerable();
            List<int> actual = new List<int>();
            foreach (var item in actualDTO)
            {
                actual.Add(item.Id);   
            }

            //Assert
            Assert.Equal(expected, actual);
        }

/*
TODO:
-We need project-bucket to reference both ways
-THE PROBLEM IS: THere is no bucket to project reference
*/

/*
        [Fact]
        public void Delete_Removes_All_Groups_In_Buckets()
        {
            //Arrange
            int expected = 0 - LargeLSH.NumberOfGroups;
            foreach (KeyValuePair<string, Bucket<IProject>> entry in LargeLSH.Map)
            {
                expected += entry.Value.Projects.Count();
            }
            //Act
            LargeLSH.Delete(AgricultureFarming);
            int actual = 0;
            foreach (KeyValuePair<string, Bucket<IProject>> entry in LargeLSH.Map)
            {
                actual += entry.Value.Projects.Count();
            }
            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Update_Throws_Exception_If_Project_Not_Inserted_Given_NonEmpty_LSH()
        {
            //Arrange
            var ComputerSecurity = new Project { Tags = new List<Tag> { ComputerScience, Security }, Id = 8, Author = null, Title = "ComputerSecurity", Description = "ComputerScienceSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC };

            //Act and Assert
            Assert.Throws<ArgumentException>(() => LargeLSH.Update(ComputerSecurity));
        }

        [Fact]
        public void Update_Throws_Exception_If_Project_Not_Inserted_Given_Empty_LSH()
        {
            //Arrange
            var TestLSH = new ProjectLSH();

            //Act & Assert
            Assert.Throws<ArgumentException>(() => TestLSH.Update(ComputerScienceAlgorithmsSecurity));
        }

        [Fact]
        public void Insert_Throws_Exception_If_Project_Exists()
        {
            Assert.Throws<ArgumentException>(() => LargeLSH.Insert(ComputerScienceAlgorithmsSecurity));
        }

        [Fact]
        public void If_signature_group_exists_it_gets_added_to_existing_bucket()
        {
            //Arrange
            var TestLSH = new ProjectLSH();
            var Signature = ComputerScienceAlgorithmsSecurity.Signature;
            var expected = new List<int>();
            var actual = new List<int>();

            var buckets = new List<string>();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < LargeLSH.NumberOfGroups; i++)
            {
                builder.Append(Signature.Hashes.ElementAt(i * 2));
                builder.Append(Signature.Hashes.ElementAt(i * 2 + 1));
                buckets.Add(builder.ToString());
                builder.Clear();
            }

            //Act
            TestLSH.Insert(ComputerScienceAlgorithmsSecurity);
            var SizeBefore = TestLSH.Map.Count();
            for (int i = 0; i < LargeLSH.NumberOfGroups; i++)
            {
                expected.Add(TestLSH.Map[buckets[i]].Projects.Count() + 1);
            }

            TestLSH.Insert(ComputerScienceAlgorithmsSecurityIdentical);
            var SizeAfter = TestLSH.Map.Count();
            for (int i = 0; i < LargeLSH.NumberOfGroups; i++)
            {
                actual.Add(TestLSH.Map[buckets[i]].Projects.Count());
            }
            //Assert
            Assert.Equal(SizeBefore, SizeAfter); //Number of buckets should be the same
            Assert.Equal(expected, actual); //The same buckets get one more group
        }

        [Fact]
        public void HashesToBucketString_Splits_Signature_To_NumberOfGroups()
        {
            //Arrange
            var TestLSH = new ProjectLSH();
            var Signature = AllUnrelatedTags.Signature;
            var expected = new string[TestLSH.NumberOfGroups];

            //Act
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < TestLSH.NumberOfGroups; i++)
            {
                builder.Append(Signature.Hashes.ElementAt(i * 2));
                builder.Append(Signature.Hashes.ElementAt(i * 2 + 1));
                expected[i] = builder.ToString();
                builder.Clear();
            }
            var actual = TestLSH.HashesToBucketString(Signature);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Insert_Throws_Exception_If_No_Tags()
        {
            //Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => LargeLSH.Insert(new Project { Tags = new List<Tag> { }, Id = 8, Author = null, Title = "ComputerSecurity", Description = "ComputerScienceSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC }));
        }

        [Fact]
        public void GetSortedByCategory_Is_Sorted_By_JaccardIndex()
        {
            //Arrange
            var expected = new List<IProject> { ComputerScienceAlgorithmsSecurityIdentical, ComputerScienceAlgorithmsSimulationSecurityIdentical, ComputerScienceSimulationAlgorithmsAgricultureIdentical, ComputerScienceSimulationAlgorithmsAgriculture }.AsEnumerable();

            //Act
            var actual = LargeLSH.GetSortedInCategory(ComputerScienceAlgorithmsSecurity).AsEnumerable();

            //Assert
            Assert.Equal(expected, actual);
        }

        //IF PROJECT DOESN'T HAVE THAT CATEGORY

        [Fact]
        public void GetSortedByCategory_Only_Consists_Of_Projects_Within_Category()
        {
            //Arrange
            var list = LargeLSH.GetSortedInCategory(SecurityProject);

            //Act
            var AllWithinCategory = true;
            foreach (IProject tagable in list)
            {
                var project = (Project)tagable;
                if (!project.Category.IsRelated(FacultyDesignDTU)) AllWithinCategory = false;
            }

            //Assert
            Assert.True(AllWithinCategory);
        }

        //FINISH THIS TEST WHEN YOU HAVE FIGURED OUT CATEGORIZABLE
        [Fact]
        public void GetSortedByCategory_Throws_Exception_If_No_Projects_Have_Category()
        {
            //Arrange
            Project test = new Project { Tags = new List<Tag> { Mathematics, Algebra, DiscreteMathematics, Calculus, Statistics, Probability, Science }, Id = 19, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };

            //Act & Assert
            //Assert.Equal(test.Category, null);
            //Assert.Throws<ArgumentException>(() => LargeLSH.Insert(test));
        }

        [Fact]
        public void GetSortedByCategory_Equals_GetSorted_Within_Category()
        {

        }

        private void insertSmallData()
        {
            AgricultureFood = new Project { Tags = new List<Tag> { Agriculture, Food }, Id = 1, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            ComputerScienceSimulationAlgorithmsAgriculture = new Project { Tags = new List<Tag> { ComputerScience, Simulation, Algorithms, Agriculture }, Id = 2, Author = null, Title = "ComputerScienceSimulationAlgorithms", Description = "ComputerScienceSimulationAlgorithmsAgriculture", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            ComputerScienceAlgorithmsSecurity = new Project { Tags = new List<Tag> { ComputerScience, Algorithms, Security }, Id = 3, Author = null, Title = "ComputerScienceAlgorithmsSecurity", Description = "ComputerScienceAlgorithmsSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            AgricultureFarming = new Project { Tags = new List<Tag> { Agriculture, Farming, Food }, Id = 4, Author = null, Title = "AgricultureFarming", Description = "AgricultureFarming", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            ComputerScienceAlgorithmsSimulationSecurity = new Project { Tags = new List<Tag> { ComputerScience, Algorithms, Simulation, Security }, Id = 5, Author = null, Title = "ComputerScienceAlgorithmsSimulationSecurity", Description = "ComputerScienceAlgorithmsSimulationSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            LSH = new ProjectLSH();
            LSH.Insert(AgricultureFarming);
            LSH.Insert(ComputerScienceSimulationAlgorithmsAgriculture);
            LSH.Insert(ComputerScienceAlgorithmsSecurity);
            LSH.Insert(AgricultureFood);
            LSH.Insert(ComputerScienceAlgorithmsSimulationSecurity);
        }
*/
        Project AgricultureFoodIdentical;
        Project ComputerScienceSimulationAlgorithmsAgricultureIdentical;
        Project ComputerScienceAlgorithmsSecurityIdentical;
        Project AgricultureFarmingIdentical;
        Project ComputerScienceAlgorithmsSimulationSecurityIdentical;

        Project FoodProject;
        Project FoodAndAgricultureProject;
        Project AgricultureSecurityProject;
        Project SecurityFoodProject;
        Project SecurityProject;

        Project ManyTagsProject;
        Project LessManyTagsProject;
        Project LessManyTagsProject2;
        Project LessManyTagsProject3;
        Project AllUnrelatedTags;
        Project MathAlgebraDiscrete;
        Project ProbabilityProject;
        Project CalcStatProbScience;
        Project AlgDiscCalcStat;
        Project MathAlgDisc;
        Project MathAlgDiscCalc;
        Project CalcStatProbScience2;
        Tag Mathematics = new Tag("Mathematics");
        Tag Algebra = new Tag("Algebra");
        Tag DiscreteMathematics = new Tag("Discrete Mathematics");
        Tag Calculus = new Tag("Calculus");
        Tag Statistics = new Tag("Statistics");
        Tag Probability = new Tag("Probability");
        Tag Science = new Tag("Science");

        static Institution ITU = new Institution { Id = 1, Title = "ITU", Description = "ITU"};
        static Institution DTU = new Institution { Id = 2, Title = "DTU", Description = "ITU" };
        static Faculty FacultyComputerScienceITU = new Faculty { Id = 3, Institution = ITU, Title = "FC ITU", Description = "ITU" };
        static Faculty FacultyDesignDTU = new Faculty { Id = 4, Institution = DTU, Title = "FD DTU", Description = "ITU" };
        static TeachingProgram DataScience = new TeachingProgram { Id = 5, Faculty = FacultyComputerScienceITU, Title = "DS", Description = "ITU", Code = "DS2021", Courses = new List<Course>()};
        static TeachingProgram InteractiveDesign = new TeachingProgram { Id = 6, Faculty = FacultyDesignDTU, Title = "ID", Description = "ITU", Code = "ID2020", Courses = new List<Course>()};
        
        static Student Villum = new Student("vson@itu.dk", ITU, "Villum", "Sonne", new List<Project>(), DataScience, new List<Course>()) {Id = 5};
        static Student Anton = new Student("antbr@itu.dk", ITU, "Anton", "Breinholt", new List<Project>(), InteractiveDesign, new List<Course>()) {Id = 4};

        Supervisor Supervisor1 = new Supervisor("troe@itu.dk", ITU, "Troels", "Jyde", new List<Project>(), FacultyComputerScienceITU, new List<Project>());

        static Course IntroductoryProgramming = new Course { Id = 7, Programs = new List<TeachingProgram> { DataScience }, Faculty = FacultyComputerScienceITU, Title = "IP", Description = "ITU", Code = "IA2002", Students = new List<Student>() {Villum}};
        static Course UIDesign = new Course { Id = 8, Programs = new List<TeachingProgram> { InteractiveDesign }, Faculty = FacultyDesignDTU, Title = "UID", Description = "ITU", Code = "19283", Students = new List<Student>(){Anton}};

        //FacultyComputerScienceITU.Institution = ITU;

    }
}
