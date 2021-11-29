using System.Collections.Generic;
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

        static string expectedComputerScienceSHA1 = "0bbb843c75b8cb93ceb9d5594e208668484448ee";
        static string expectedComputerScienceSHA256 = "13a5670cc77404aa14b878e0b1a82651e705d21f7ba2956918a1406b26871a1f";
        static string expectedComputerScienceMD5 = "80c18801f6ce7e664f788b70032cdcd1";
        static string expectedComputerScienceSHA384 = "8c067704ddd0e69aee028cfec66fe56f1d192a65ce51e7512adee32e5c0b53e4cb76b92af6ca49302301e4a775a8016c";
        static string expectedComputerScienceSHA512 = "fdb146ddd1f0fc4d95730a054616a100b2c2521b83c615a035e9e7c6fb31bf56fda50ba694f1ef84675801ad344cdc76e9027c3248af941ff99cc0163b247181";
        static string expectedComputerScienceNoHash = "computer science";

        static List<string> expectedAgricultureComputerScience = new List<string> { expectedComputerScienceSHA1, expectedComputerScienceSHA256, expectedComputerScienceMD5, expectedComputerScienceSHA384, expectedAgricultureSHA512, expectedAgricultureNoHash };




        [Fact]
        public void Signature_constructor_returns_Signature_given_list_of_one_tag()
        {
            //Arrange
            var expected = expectedAgriculture.AsReadOnly();
            var tags = new List<Tag> { new Tag { Id = 1, Name = "agriculture" } }.AsReadOnly();
            //Act
            var actual = new Signature(tags);

            //Assert
            Assert.Equal(expected, actual.Hashes);
        }

        [Fact]
        public void Signature_constructor_returns_Signature_with_correct_hashes_given_list_of_multiple_tags()
        {
            //Arrange
            var expected = expectedAgricultureComputerScience.AsReadOnly();

            var tags = new List<Tag> { new Tag { Id = 1, Name = "agriculture" }, new Tag { Id = 2, Name = "computer science" } }.AsReadOnly();
            //Act
            var actual = new Signature(tags);

            //Assert
            Assert.Equal(expected, actual.Hashes);
        }
    }
}