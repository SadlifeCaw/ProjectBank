using System;
using System.Linq;
using System.Text;
using Xunit;
using System.Collections.Generic;
using ProjectBank.Infrastructure;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure.ReferenceSystem;


namespace ReferenceSystem.Tests
{
    public class LocalitySensitiveHashTableTests
    {
        //Tests:
        //
        //HashestobucketString splits up a signature correctly
        //Insert throws exception for project with no tags
        //Insert works for projects with a lot of tags
        //Delete and update won't work, if project doesn't exist
        //Test insert into empty map

        Tag Agriculture = new Tag { Name = "Agriculture" };
        Tag ComputerScience = new Tag { Name = "Computer Science" };
        Tag Security = new Tag { Name = "Security" };
        Tag Algorithms = new Tag { Name = "Algorithms" };
        Tag Simulation = new Tag { Name = "Simulation" };
        Tag Food = new Tag { Name = "Food" };
        Tag Farming = new Tag { Name = "Farming" };

        ITagable AgricultureFood;
        ITagable ComputerScienceSimulationAlgorithmsAgriculture;
        ITagable ComputerScienceAlgorithmsSecurity;
        ITagable AgricultureFarming;
        ITagable ComputerScienceAlgorithmsSimulationSecurity;

        LocalitySensitiveHashTable LSH;
        LocalitySensitiveHashTable LargeLSH;

        IList<LocalitySensitiveHashTable> LSHList;

        public LocalitySensitiveHashTableTests()
        {
            //SMALL LSH:
            AgricultureFood = new Project { Tags = new List<Tag> { Agriculture, Food }, Id = 1, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            ComputerScienceSimulationAlgorithmsAgriculture = new Project { Tags = new List<Tag> { ComputerScience, Simulation, Algorithms, Agriculture }, Id = 2, Author = null, Title = "ComputerScienceSimulationAlgorithms", Description = "ComputerScienceSimulationAlgorithmsAgriculture", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            ComputerScienceAlgorithmsSecurity = new Project { Tags = new List<Tag> { ComputerScience, Algorithms, Security }, Id = 3, Author = null, Title = "ComputerScienceAlgorithmsSecurity", Description = "ComputerScienceAlgorithmsSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            AgricultureFarming = new Project { Tags = new List<Tag> { Agriculture, Farming, Food }, Id = 4, Author = null, Title = "AgricultureFarming", Description = "AgricultureFarming", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            ComputerScienceAlgorithmsSimulationSecurity = new Project { Tags = new List<Tag> { ComputerScience, Algorithms, Simulation, Security }, Id = 5, Author = null, Title = "ComputerScienceAlgorithmsSimulationSecurity", Description = "ComputerScienceAlgorithmsSimulationSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC };

            LSH = new LocalitySensitiveHashTable();
            LSH.Insert(AgricultureFarming);
            LSH.Insert(ComputerScienceSimulationAlgorithmsAgriculture);
            LSH.Insert(ComputerScienceAlgorithmsSecurity);
            LSH.Insert(AgricultureFood);
            LSH.Insert(ComputerScienceAlgorithmsSimulationSecurity);


            //LARGE LSH:
            AgricultureFoodIdentical = new Project { Tags = new List<Tag> { Agriculture, Food }, Id = 6, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            ComputerScienceSimulationAlgorithmsAgricultureIdentical = new Project { Tags = new List<Tag> { ComputerScience, Simulation, Algorithms, Agriculture }, Id = 7, Author = null, Title = "ComputerScienceSimulationAlgorithms", Description = "ComputerScienceSimulationAlgorithmsAgriculture", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            ComputerScienceAlgorithmsSecurityIdentical = new Project { Tags = new List<Tag> { ComputerScience, Algorithms, Security }, Id = 8, Author = null, Title = "ComputerScienceAlgorithmsSecurityIdentical", Description = "ComputerScienceAlgorithmsSecurityIdentical", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            AgricultureFarmingIdentical = new Project { Tags = new List<Tag> { Agriculture, Farming, Food }, Id = 9, Author = null, Title = "AgricultureFarming", Description = "AgricultureFarming", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            ComputerScienceAlgorithmsSimulationSecurityIdentical = new Project { Tags = new List<Tag> { ComputerScience, Algorithms, Simulation, Security }, Id = 10, Author = null, Title = "ComputerScienceAlgorithmsSimulationSecurity", Description = "ComputerScienceAlgorithmsSimulationSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC };

            FoodProject = new Project { Tags = new List<Tag> { Food }, Id = 11, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            FoodAndAgricultureProject = new Project { Tags = new List<Tag> { Food, Agriculture }, Id = 12, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            AgricultureSecurityProject = new Project { Tags = new List<Tag> { Agriculture, Security }, Id = 13, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            SecurityFoodProject = new Project { Tags = new List<Tag> { Food, Security }, Id = 14, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            SecurityProject = new Project { Tags = new List<Tag> { Security }, Id = 15, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };

            ManyTagsProject = new Project { Tags = new List<Tag> { Agriculture, ComputerScience, Security, Algorithms, Simulation, Food, Farming }, Id = 16, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            LessManyTagsProject = new Project { Tags = new List<Tag> { Agriculture, ComputerScience, Security, Simulation, Food, Farming }, Id = 17, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            LessManyTagsProject2 = new Project { Tags = new List<Tag> { ComputerScience, Security, Simulation, Food, Farming, Algorithms }, Id = 18, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            LessManyTagsProject3 = new Project { Tags = new List<Tag> { ComputerScience, Security, Simulation, Food, Algorithms, Agriculture }, Id = 19, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };

            AllUnrelatedTags = new Project { Tags = new List<Tag> { Mathematics, Algebra, DiscreteMathematics, Calculus, Statistics, Probability, Science }, Id = 19, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            MathAlgebraDiscrete = new Project { Tags = new List<Tag> { Mathematics, Algebra, DiscreteMathematics }, Id = 20, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            ProbabilityProject = new Project { Tags = new List<Tag> { Probability }, Id = 21, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            CalcStatProbScience = new Project { Tags = new List<Tag> { Calculus, Statistics, Probability, Science }, Id = 22, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            AlgDiscCalcStat = new Project { Tags = new List<Tag> { Algebra, DiscreteMathematics, Calculus, Statistics }, Id = 23, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            MathAlgDisc = new Project { Tags = new List<Tag> { Mathematics, Algebra, DiscreteMathematics }, Id = 24, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            MathAlgDiscCalc = new Project { Tags = new List<Tag> { Mathematics, Algebra, DiscreteMathematics, Calculus }, Id = 25, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            CalcStatProbScience2 = new Project { Tags = new List<Tag> { Calculus, Statistics, Probability, Science }, Id = 26, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC }; LargeLSH = new LocalitySensitiveHashTable();

            LargeLSH = new LocalitySensitiveHashTable();
            LargeLSH.Insert(AgricultureFarmingIdentical);
            LargeLSH.Insert(ComputerScienceSimulationAlgorithmsAgricultureIdentical);
            LargeLSH.Insert(ComputerScienceAlgorithmsSecurityIdentical);
            LargeLSH.Insert(AgricultureFoodIdentical);
            LargeLSH.Insert(ComputerScienceAlgorithmsSimulationSecurityIdentical);
            LargeLSH.Insert(AgricultureFarming);
            LargeLSH.Insert(ComputerScienceSimulationAlgorithmsAgriculture);
            LargeLSH.Insert(ComputerScienceAlgorithmsSecurity);
            LargeLSH.Insert(AgricultureFood);
            LargeLSH.Insert(ComputerScienceAlgorithmsSimulationSecurity);
            LargeLSH.Insert(FoodProject);
            LargeLSH.Insert(FoodAndAgricultureProject);
            LargeLSH.Insert(AgricultureSecurityProject);
            LargeLSH.Insert(SecurityFoodProject);
            LargeLSH.Insert(SecurityProject);
            LargeLSH.Insert(ManyTagsProject);
            LargeLSH.Insert(LessManyTagsProject);
            LargeLSH.Insert(LessManyTagsProject2);
            LargeLSH.Insert(LessManyTagsProject3);
            LargeLSH.Insert(AllUnrelatedTags);
            LargeLSH.Insert(MathAlgebraDiscrete);
            LargeLSH.Insert(ProbabilityProject);
            LargeLSH.Insert(CalcStatProbScience);
            LargeLSH.Insert(AlgDiscCalcStat);
            LargeLSH.Insert(MathAlgDisc);
            LargeLSH.Insert(MathAlgDiscCalc);
            LargeLSH.Insert(CalcStatProbScience2);

            LSHList = new List<LocalitySensitiveHashTable>();
            LSHList.Add(LSH); //Index 0
            LSHList.Add(LargeLSH); //Index 1
        }

        [Fact]
        public void Update_Replaces_Project_With_Updated()
        {
            //Arrange
            var tagable = AgricultureFarming;
            AgricultureFarming.Tags.Append(Food);
            IEnumerable<Tag> expectedTags = new List<Tag> { Agriculture, Farming, Food }.AsEnumerable();

            //Act
            LSH.Update(tagable);

            IEnumerable<Tag> actualTags = new List<Tag> { };

            foreach (string buckestString in LSH.HashesToBucketString(tagable.Signature))
            {
                foreach (ITagable tagableInBucket in LSH.Map[buckestString].Projects)
                {
                    if (tagable == tagableInBucket)
                    {
                        actualTags = tagableInBucket.Tags.AsEnumerable();
                        break;
                    }
                }
            }

            //Assert
            foreach (string buckestString in LSH.HashesToBucketString(tagable.Signature))
            {
                Assert.True(LSH.Map[buckestString].Projects.Contains(tagable));
            }

            Assert.Equal(expectedTags, actualTags);
        }

        //When calling Get, all projects have at least two simular tags
        //Only works for k = 6
        [Fact]
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
                var bucket = LSH.Map[str];
                foreach (ITagable project in bucket.Projects)
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
        public void LARGE_Get_Returns_Tagables_With_One_Or_More_Common_Tags()
        {
            //Arrange
            var Singature = ComputerScienceAlgorithmsSecurity.Signature;
            var buckets = new List<string>();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                builder.Append(Singature.Hashes.ElementAt(i*2));
                builder.Append(Singature.Hashes.ElementAt(i*2 + 1));
                buckets.Add(builder.ToString());
                builder.Clear();
            }
            
            //Act & Assert
            foreach (var str in buckets)
            {
                var bucket = LargeLSH.Map[str];
                foreach (ITagable project in bucket.Projects)
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
        public void Projects_Within_Bucket_Has_One_Or_More_Common_Tags()
        {
            string bucketString = LargeLSH.HashesToBucketString(ComputerScienceAlgorithmsSecurity.Signature)[0];
            Bucket bucket = LargeLSH.Map[bucketString];

            foreach (ITagable tagable in bucket.Projects)
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
        [Fact]
        public void Insert_Adds_BucketGroup_If_Non_Existant()
        {
            //Arrange 
            var TestLSH = new LocalitySensitiveHashTable();
            var AgriFarmSignature = new Signature(AgricultureFarming.Tags);
            var AgriFoodSignature = new Signature(AgricultureFood.Tags);

            //Act
            TestLSH.Insert(AgricultureFarming);
            var bucketSizeBeforeInsert = TestLSH.Map.Count();
            TestLSH.Insert(AgricultureFood);
            var bucketSizeAfterInsert = TestLSH.Map.Count();
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

        [Fact]
        public void SMALL_GetSorted_returns_similar_projects_sorted_by_highest_jaccardindex()
        {
            //Arrange
            var expected = new List<ITagable> { ComputerScienceAlgorithmsSimulationSecurity, ComputerScienceSimulationAlgorithmsAgriculture }.AsEnumerable();

            //Act
            var actual = LSH.GetSorted(ComputerScienceAlgorithmsSecurity).AsEnumerable();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LARGE_GetSorted_returns_similar_projects_sorted_by_highest_jaccardindex()
        {
            //Arrange
            var expected = new List<ITagable> { ComputerScienceAlgorithmsSecurityIdentical, ComputerScienceAlgorithmsSimulationSecurityIdentical, ComputerScienceAlgorithmsSimulationSecurity, LessManyTagsProject2, ComputerScienceSimulationAlgorithmsAgricultureIdentical, ComputerScienceSimulationAlgorithmsAgriculture }.AsEnumerable();

            //Act
            var actual = LargeLSH.GetSorted(ComputerScienceAlgorithmsSecurity).AsEnumerable();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Delete_Removes_All_Groups_In_Buckets()
        {
            //Arrange
            int expected = 0 - LargeLSH.NumberOfGroups;
            foreach (KeyValuePair<string, Bucket> entry in LargeLSH.Map)
            {
                expected += entry.Value.Projects.Count();
            }
            //Act
            LargeLSH.Delete(AgricultureFarming);
            int actual = 0;
            foreach (KeyValuePair<string, Bucket> entry in LargeLSH.Map)
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
        public void Update_Throws_Exception_If_Project_Not_Inserted_Given_Empty_LSH() {
            //Arrange
            var TestLSH = new LocalitySensitiveHashTable();
            
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
            var TestLSH = new LocalitySensitiveHashTable();
            var Signature = ComputerScienceAlgorithmsSecurity.Signature;
            var expected = new List<int>();
            var actual = new List<int>();
            
            var buckets = new List<string>();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < LargeLSH.NumberOfGroups; i++)
            {
                builder.Append(Signature.Hashes.ElementAt(i*2));
                builder.Append(Signature.Hashes.ElementAt(i*2 + 1));
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
            var TestLSH = new LocalitySensitiveHashTable();
            var Signature = AllUnrelatedTags.Signature;
            var expected = new string[TestLSH.NumberOfGroups];
            
            //Act
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < TestLSH.NumberOfGroups; i++) 
            {
                builder.Append(Signature.Hashes.ElementAt(i*2));
                builder.Append(Signature.Hashes.ElementAt(i*2 + 1));
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
            Assert.Throws<ArgumentException>(() => LargeLSH.Insert(new Project { Tags = new List<Tag> {}, Id = 8, Author = null, Title = "ComputerSecurity", Description = "ComputerScienceSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC }));      
        }

        private void insertSmallData()
        {
            AgricultureFood = new Project { Tags = new List<Tag> { Agriculture, Food }, Id = 1, Author = null, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            ComputerScienceSimulationAlgorithmsAgriculture = new Project { Tags = new List<Tag> { ComputerScience, Simulation, Algorithms, Agriculture }, Id = 2, Author = null, Title = "ComputerScienceSimulationAlgorithms", Description = "ComputerScienceSimulationAlgorithmsAgriculture", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            ComputerScienceAlgorithmsSecurity = new Project { Tags = new List<Tag> { ComputerScience, Algorithms, Security }, Id = 3, Author = null, Title = "ComputerScienceAlgorithmsSecurity", Description = "ComputerScienceAlgorithmsSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            AgricultureFarming = new Project { Tags = new List<Tag> { Agriculture, Farming, Food }, Id = 4, Author = null, Title = "AgricultureFarming", Description = "AgricultureFarming", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            ComputerScienceAlgorithmsSimulationSecurity = new Project { Tags = new List<Tag> { ComputerScience, Algorithms, Simulation, Security }, Id = 5, Author = null, Title = "ComputerScienceAlgorithmsSimulationSecurity", Description = "ComputerScienceAlgorithmsSimulationSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC };
            LSH = new LocalitySensitiveHashTable();
            LSH.Insert(AgricultureFarming);
            LSH.Insert(ComputerScienceSimulationAlgorithmsAgriculture);
            LSH.Insert(ComputerScienceAlgorithmsSecurity);
            LSH.Insert(AgricultureFood);
            LSH.Insert(ComputerScienceAlgorithmsSimulationSecurity);
        }

        ITagable AgricultureFoodIdentical;
        ITagable ComputerScienceSimulationAlgorithmsAgricultureIdentical;
        ITagable ComputerScienceAlgorithmsSecurityIdentical;
        ITagable AgricultureFarmingIdentical;
        ITagable ComputerScienceAlgorithmsSimulationSecurityIdentical;

        ITagable FoodProject;
        ITagable FoodAndAgricultureProject;
        ITagable AgricultureSecurityProject;
        ITagable SecurityFoodProject;
        ITagable SecurityProject;

        ITagable ManyTagsProject;
        ITagable LessManyTagsProject;
        ITagable LessManyTagsProject2;
        ITagable LessManyTagsProject3;

        ITagable AllUnrelatedTags;
        ITagable MathAlgebraDiscrete;
        ITagable ProbabilityProject;
        ITagable CalcStatProbScience;
        ITagable AlgDiscCalcStat;
        ITagable MathAlgDisc;
        ITagable MathAlgDiscCalc;
        ITagable CalcStatProbScience2;
        Tag Mathematics = new Tag { Name = "Mathematics" };
        Tag Algebra = new Tag { Name = "Algebra" };
        Tag DiscreteMathematics = new Tag { Name = "Discrete Mathematics" };
        Tag Calculus = new Tag { Name = "Calculus" };
        Tag Statistics = new Tag { Name = "Statistics" };
        Tag Probability = new Tag { Name = "Probability" };
        Tag Science = new Tag { Name = "Science" };

    }
}
