using System.Collections.Generic;
using System;
using Xunit;
using ProjectBank.Infrastructure.Entities;
using ProjectBank.Infrastructure;
namespace ReferenceSystem.Tests
{

    public class SignatureTests
    {
        static string expectedAgricultureSHA1 = "96c842e9da8fd2e3d76890658ce7e43d08de90c2";
        static string expectedAgricultureSHA256 = "3f52a423f8ebd4ccad56c627fa1287d3286e15e20968d590bfbbbb6d53162a1c";
        static string expectedAgricultureMD5 = "f8ea9e07f542c0cdf0805e5f30c76cb6";
        static string expectedAgricultureSHA384 = "ff5facdc5d5ee3e511b93375d0f73aab14d0b4afce62d38007c76a089fcf103ef8da82b360a1e42ae654a49ac270ba65";
        static string expectedAgricultureSHA512 = "8e92a5e09ab90a391102756ea6496af289b5eefd9c6ffa3453b1987f725381953ffa3b43fc63d7dd5b6af3859e40e3bf654da02b722392f10d204f354e2d9e1e";
        static string expectedAgricultureNoHash = "agriculture";
        static List<string> expectedAgriculture = new List<string> { expectedAgricultureSHA1, expectedAgricultureSHA256, expectedAgricultureMD5, expectedAgricultureSHA384, expectedAgricultureSHA512, expectedAgricultureNoHash };

        //computer science
        static string expectedComputerScienceSHA1 = "0bbb843c75b8cb93ceb9d5594e208668484448ee";
        static string expectedComputerScienceSHA256 = "13a5670cc77404aa14b878e0b1a82651e705d21f7ba2956918a1406b26871a1f";
        static string expectedComputerScienceMD5 = "80c18801f6ce7e664f788b70032cdcd1";
        static string expectedComputerScienceSHA384 = "8c067704ddd0e69aee028cfec66fe56f1d192a65ce51e7512adee32e5c0b53e4cb76b92af6ca49302301e4a775a8016c";
        static string expectedComputerScienceSHA512 = "fdb146ddd1f0fc4d95730a054616a100b2c2521b83c615a035e9e7c6fb31bf56fda50ba694f1ef84675801ad344cdc76e9027c3248af941ff99cc0163b247181";
        static string expectedComputerScienceNoHash = "computer science";

        //algorithms

        static string expectedAlgorithmseSHA1 = "5f0f56a9485a7a39786b596e02a73af72715e320";
        static string expectedAlgorithmsSHA256 = "1ba71009ae9a0077c7acd2c8786fd8467b6351552a427b15ad1982f229702934";
        static string expectedAlgorithmsMD5 = "66270707424a729c3e557fceb03f45c9";
        static string expectedAlgorithmsSHA384 = "fce1a472deede1af4667d79e2ed4b37549f76f1dd5875e29904dd23d55e87f7543a1ac43bbd0bd3e9d2aea33797324de";
        static string expectedAlgorithmsSHA512 = "2a809fd3f1b58aba92889c4f063dd85ae9e0e00276b01e1705df65bceec13072fa2c03ce641dfd63e271113a94c799afcb261e992a00d6f85fb34b8f0a1a7c48";
        static string expectedAlgorithmsNoHash = "algorithms";

        //databases

        static string expectedDatabaseseSHA1 = "0167bbf5aa9e9c0c005b2c62e0191b73a1e34df0";
        static string expectedDatabasesSHA256 = "1d446138494b19b4281eb1692354038c2cde85d09ce9982fef6b904bca4f6c72";
        static string expectedDatabasesMD5 = "e61ce3062cb76770658896b778ad06cd";
        static string expectedDatabasesSHA384 = "df57d95fe7c741024902ed9eecde22f7a68f47332f5e2062c9fcd02fd5b9e1a88985e503150fe7c6c899a7267703e243";
        static string expectedDatabasesSHA512 = "11ab34ebc30b79bfbc551909d52ff311e59acd88766ee9ee9b625fd0660bde0501b23a5abd7bc1f378fbb61b315771095c56e9329cee27ac186041c27352882c";
        static string expectedDatabasesNoHash = "databases";
        static List<string> expectedAgricultureComputerScience = new List<string> { expectedDatabaseseSHA1, expectedComputerScienceSHA256, expectedAlgorithmsMD5, expectedComputerScienceSHA384, expectedDatabasesSHA512, expectedAgricultureNoHash };

        [Fact]
        public void Constructor_Throws_Exception_Given_No_Tags()
        {
            //Arrange
            var tags = new List<Tag>{}.AsReadOnly();

            //Act & Assert
            Assert.Throws<ArgumentException>(() => new Signature(tags));   
        }
        
        [Fact]
        public void Constructor_Returns_Signature_Given_One_Tag()
        {
            //Arrange
            var expected = expectedAgriculture.AsReadOnly();
            var tags = new List<Tag> { new Tag ("agriculture") }.AsReadOnly();
            //Act
            var actual = new Signature(tags);

            //Assert
            Assert.Equal(expected, actual.Hashes);
        }

        [Fact]
        public void Constructor_Returns_Signature_With_Correct_Hashes_Given_Multiple_Tags()
        {
            //Arrange
            var expected = expectedAgricultureComputerScience.AsReadOnly();
            var tags = new List<Tag> { new Tag("agriculture"), new Tag("computer science"), new Tag("algorithms"), new Tag("databases")}.AsReadOnly();
            
            //Act
            var actual = new Signature(tags);

            //Assert
            Assert.Equal(expected, actual.Hashes);
        }
    }
}