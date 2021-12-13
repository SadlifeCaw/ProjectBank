using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectBank.Server.Controllers;
using ProjectBank.Core.EF.DTO;
using ProjectBank.Core.EF.Repository;
using ProjectBank.Infrastructure;
using ProjectBank.Core;
using System.Collections.Generic;

namespace ProjectBank.Tests.Controller.Tests;

public class TagControllerTests
{
    [Fact]
    public async Task Get_given_non_existing_returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<TagController>>();
        var repository = new Mock<ITagRepository>();
        repository.Setup(m => m.ReadTagByIDAsync(42)).ReturnsAsync(default(TagDTO));
        var controller = new TagController(logger.Object, repository.Object);

        // Act
        var response = await controller.GetTag(42);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task Get_given_existing_returns_character()
    {
        // Arrange
        var logger = new Mock<ILogger<TagController>>();
        var repository = new Mock<ITagRepository>();
        var tag = new TagDTO(1, "Agriculture");
        repository.Setup(m => m.ReadTagByIDAsync(1)).ReturnsAsync(tag);
        var controller = new TagController(logger.Object, repository.Object);

        // Act
        var response = await controller.GetTag(1);

        // Assert
        Assert.Equal(tag, response.Value);
    }
}