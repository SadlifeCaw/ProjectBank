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

        public LocalitySensitiveHashTableTests()
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

        [Fact]
        public void Update_given_old_and_new_ITagable_updates_old_to_new()
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
        public void Get_Returns_Tagables_With_One_Or_More_Common_Tags()
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
            //LSH.Map
        }

        //Projects within a bucket have at least N simular tags
        [Fact]
        public void Projects_Within_Bucket_Has_One_Or_More_Common_Tags()
        {
            string bucketString = LSH.HashesToBucketString(ComputerScienceAlgorithmsSecurity.Signature)[0];
            Bucket bucket = LSH.Map[bucketString];

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
        public void GetSorted_returns_similar_projects_sorted_by_highest_jaccardindex()
        {
            //Arrange
            var expected = new List<ITagable> { ComputerScienceAlgorithmsSimulationSecurity, ComputerScienceSimulationAlgorithmsAgriculture }.AsEnumerable();

            //Act
            var actual = LSH.GetSorted(ComputerScienceAlgorithmsSecurity).AsEnumerable();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Delete_Removes_Project()
        {
            //Arrange
            int expected = -3;
            foreach (KeyValuePair<string, Bucket> entry in LSH.Map)
            {
                expected += entry.Value.Projects.Count();
            }
            //Act
            LSH.Delete(AgricultureFarming);
            int actual = 0;
            foreach (KeyValuePair<string, Bucket> entry in LSH.Map)
            {
                actual += entry.Value.Projects.Count();
            }
            //Assert
            Assert.Equal(expected, actual); //3 DEPENDENT ON K, N, B FROM LSH
        }

        //EXCEPTION METHODS:

        [Fact]
        public void Update_And_Delete_Throws_Exception_If_Project_Not_Inserted()
        {
            //Arrange
            var ComputerSecurity = new Project { Tags = new List<Tag> { ComputerScience, Security }, Id = 8, Author = null, Title = "ComputerSecurity", Description = "ComputerScienceSecurity", Status = ProjectBank.Core.ProjectStatus.PUBLIC };            
            var TestLSH = new LocalitySensitiveHashTable();
            //Act and Assert
            Assert.Throws<ArgumentException>(() => LSH.Update(ComputerSecurity));
            Assert.Throws<ArgumentException>(() => TestLSH.Update(ComputerSecurity));
        }

        [Fact]
        public void Insert_Throws_Exception_If_Project_Exists()
        {
            Assert.Throws<ArgumentException>(() => LSH.Insert(ComputerScienceAlgorithmsSecurity));
        }

        [Fact]
        public void If_signature_group_exists_it_gets_added_to_existing_bucket()
        {
            
        }
    }
}