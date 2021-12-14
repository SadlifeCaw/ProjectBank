namespace ProjectBank.Tests.Controllers.Tests;

public class UsersControllers
{
    [Fact]
    public async Task Get_given_existing_returns_given()
    {
        //Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();

        var user = new StudentDTO(1, "jens@gmail.com", "Jens", "Jensen", "SWU2020", "ITU", new List<int>(), new List<int>());
        repository.Setup(m => m.ReadByID(1)).ReturnsAsync(user);

        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var response = await controller.Get(1);

        // Assert
        Assert.Equal(user, response.Value);
    }

    [Fact]
    public async Task Get_given_non_existing_returns_NotFound()
    {
        //Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        var controller = new UsersController(logger.Object, repository.Object);

        // Act
        var response = await controller.Get(100);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    //makes sure that the controller doesn't return the student entity when looking for supervisor,
    //even if given a proper ID 
    [Fact]
    public async Task Get_given_supervisor_non_existing_returns_NotFound()
    {
        //Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        var controller = new UsersController(logger.Object, repository.Object);

        var user = new StudentDTO(1, "jens@gmail.com", "Jens", "Jensen", "SWU2020", "ITU", new List<int>(), new List<int>());
        repository.Setup(m => m.ReadByID(1)).ReturnsAsync(user);

        // Act
        var response = await controller.GetSupervisor(1);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    //makes sure that the controller doesn't return the supervisor entity when looking for student,
    //even if given a proper ID 
    [Fact]
    public async Task Get_given_student_non_existing_returns_NotFound()
    {
        //Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        var controller = new UsersController(logger.Object, repository.Object);

        var user = new SupervisorDTO(1, "jens@gmail.com", "Jens", "Jensen", "Computer Sciecne", "ITU", new List<int>(), new List<int>());
        repository.Setup(m => m.ReadByID(1)).ReturnsAsync(user);

        // Act
        var response = await controller.GetStudent(1);

        // Assert
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async Task Get_given_student_existing_returns_given()
    {
        //Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        var controller = new UsersController(logger.Object, repository.Object);

        var user = new StudentDTO(1, "jens@gmail.com", "Jens", "Jensen", "SWU2020", "ITU", new List<int>(), new List<int>());
        repository.Setup(m => m.ReadStudent(1)).ReturnsAsync(user);

        // Act
        var response = await controller.GetStudent(1);

        // Assert
        Assert.Equal(user, response.Value);
    }

    [Fact]
    public async Task Get_given_Supervisor_existing_returns_given()
    {
        //Arrange
        var logger = new Mock<ILogger<UsersController>>();
        var repository = new Mock<IUserRepository>();
        var controller = new UsersController(logger.Object, repository.Object);

        var user = new SupervisorDTO(1, "jens@gmail.com", "Jens", "Jensen", "Computer Science", "ITU", new List<int>(), new List<int>());
        repository.Setup(m => m.ReadSupervisor(1)).ReturnsAsync(user);

        // Act
        var response = await controller.GetSupervisor(1);

        // Assert
        Assert.Equal(user, response.Value);
    }
}