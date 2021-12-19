namespace ProjectBank.Tests.Controllers.Tests;

public class ProjectReferenceControllerTests
{
    /*[Fact]
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
    }*/

   // [Fact]
    /*public async Task Get_given_existing_returns_given()
    {
        // Arrange
        var logger = new Mock<ILogger<ProjectReferenceController>>();
        var projectRepository = new Mock<IProjectRepository>();
        var bucketRepository = new Mock<IBucketRepository>();
        var tagRepository = new Mock<ITagRepository>();

        var tag = new TagDTO(1, "Agriculture");
        var project = new ProjectDTO(42, 1, "My project", "My description", ProjectStatus.PUBLIC, 5, 1, new List<int>() { 1 }, new List<int>() { 1 }, new List<int>() { 1 });
        var projectt = new Project(42, 1, "My project", "My description", ProjectStatus.PUBLIC, 5, 1, new List<int>() { 1 }, new List<int>() { 1 }, new List<int>() { 1 });
        //AgricultureFood = new Project { Category = ITU, Tags = new List<Tag> { Agriculture, Food }, Id = 1, Author = Supervisor1, Title = "AgricultureFood", Description = "AgricultureFood", Status = ProjectBank.Core.ProjectStatus.PUBLIC, Buckets = new List<ProjectBucket>(), Users = new List<User>(), Collaborators = new List<Supervisor>(), MaxStudents = 5};
        var project2 = new ProjectDTO(43, 1, "My project", "My description", ProjectStatus.PUBLIC, 5, 1, new List<int>() { 1 }, new List<int>() { 1 }, new List<int>() { 1 });
        var project22 = new Project(43, 1, "My project", "My description", ProjectStatus.PUBLIC, 5, 1, new List<int>() { 1 }, new List<int>() { 1 }, new List<int>() { 1 });
        var bucket1 = new BucketDTO(new HashSet<int>(){43}, "");
        var bucket2 = new BucketDTO(new HashSet<int>(){43}, "");
        var bucket3 = new BucketDTO(new HashSet<int>(){43}, "");
        
        tagRepository.Setup(m => m.ReadTagByIDAsync(1)).ReturnsAsync(tag);
        projectRepository.Setup(m => m.ReadByIDAsync(42)).ReturnsAsync(project);
        projectRepository.Setup(m => m.ReadByIDAsync(43)).ReturnsAsync(project2);
        bucketRepository.Setup(m => m.ReadBucketByIDAsync(1)).ReturnsAsync(bucket1);
        bucketRepository.Setup(m => m.ReadBucketByIDAsync(2)).ReturnsAsync(bucket1);
        bucketRepository.Setup(m => m.ReadBucketByIDAsync(3)).ReturnsAsync(bucket1);

        var LSH = new LocalitySensitiveHashTable<Project>(bucketRepository.Object, projectRepository.Object, tagRepository.Object);
        await LSH.Insert(project);
        var controller = new ProjectReferenceController(logger.Object, LSH);

        // Act
        var response = await controller.Get(42, 10);

        // Assert
        Assert.Equal(1, response.Count());
    }*/
}