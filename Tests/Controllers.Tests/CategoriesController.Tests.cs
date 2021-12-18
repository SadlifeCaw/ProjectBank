namespace ProjectBank.Tests.Controllers.Tests;

public class CategoriesControllerTests
{
    [Fact]
    public async Task Get_given_non_existing_returns_NotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();
        repository.Setup(m => m.ReadByIDAsync(42)).ReturnsAsync(default(ProjectDTO));
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var response = await controller.Get(42);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task Get_given_existing_returns_given()
    {
        // Arrange
        var logger = new Mock<ILogger<ProjectsController>>();
        var repository = new Mock<IProjectRepository>();
        var project = new ProjectDTO(42, 1, "My project", "My description", ProjectStatus.PUBLIC, 5, 1, new List<int>(), new List<int>(), new List<int>());
        repository.Setup(m => m.ReadByIDAsync(42)).ReturnsAsync(project);
        var controller = new ProjectsController(logger.Object, repository.Object);

        // Act
        var response = await controller.Get(42);

        // Assert
        Assert.Equal(project, response.Value);
    }
}
