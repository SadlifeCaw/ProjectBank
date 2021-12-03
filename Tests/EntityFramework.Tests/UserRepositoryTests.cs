using Xunit;


namespace EntityFramework.Tests;

public class UserRepositoryTest : IDisposable
{
    private readonly ProjectBankContext _context;
    private readonly UserRepository _repository;
    private bool disposed;

    public UserRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();

        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();

        var P1 = new List<int>();
        var P2 = new List<int>();

        Institution I1 = new Institution{Id = 3, Title = "IT-Universitetet", Description = "Informations Teknologiens Universitet"};

        context.Institutions.AddRange(
            I1
        );  

        context.Users.AddRange(
            new Student{Id = 1, Email = "bob@email.com", Institution = I1, FirstName = "Bob", LastName = "Smith", Projects = null, Program = null},
            new Supervisor{Id = 2, Email = "alice@email.com", Institution = I1, FirstName = "Alice", LastName = "Williams", Projects = null, Faculty = null}
        );

        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async void CreateAsync_creates_new_student_with_generated_id() 
    {
        //Arrange
        var studentDTO = new StudentCreateDTO
        {
            Email = "john@email.com",
            FirstName = "John",
            LastName = "Johnson",
            ProgramCode = "SWU"
        };


        //Act
        var created = await _repository.CreateAsync(studentDTO);

        if(created.Item1 == Response.Conflict)
        {
            Assert.False(true);
        }

        var i = created.Item2;

        //Assert
        Assert.Equal(Response.Created, created.Item1);
        Assert.Equal(3, i.Id);
        Assert.Equal("john@email.com", i.Email);
        Assert.Equal("John", i.FirstName);
        Assert.Equal("Johnson", i.LastName);
        Assert.Equal("SWU", i.ProgramCode);
    }

    [Fact]
    public async void CreateAsync_creates_new_supervisor_with_generated_id() 
    {
        //Arrange
        var P1 = new List<int>();
        var supervisorDTO = new SupervisorCreateDTO
        {
            Email = "john@email.com",
            FirstName = "John",
            LastName = "Johnson",
            ProjectIDs = P1
        };


        //Act
        var created = await _repository.CreateAsync(supervisorDTO);

        if(created.Item1 == Response.Conflict)
        {
            Assert.False(true);
        }

        var i = created.Item2;

        //Assert
        Assert.Equal(Response.Created, created.Item1);
        Assert.Equal(3, i.Id);
        Assert.Equal("james@email.com", i.Email);
        Assert.Equal("James", i.FirstName);
        Assert.Equal("Brown", i.LastName);
        Assert.Equal(null, i.ProjectIDs);
    }

    [Fact]
    public async void ReadByID_provided_ID_does_not_exist_returns_Null()
    {
        var nonExisting = await _repository.ReadByID(42);

        Assert.Equal(null, nonExisting);
    }

    [Fact]
    public async void ReadByID_provided_ID_exists_returns_User()
    {
        var user = await _repository.ReadByID(1);

        Assert.Equal(1, user.Id);
        Assert.Equal("bob@email.com", user.Email);
        Assert.Equal("Bob", user.FirstName);
        Assert.Equal("Smith", user.LastName);
    }

    [Fact]
    public async void ReadByEmail_provided_email_does_not_exist_returns_Null()
    {
        var nonExisting = await _repository.ReadByEmail("notExisting@email.com");

        Assert.Equal(null, nonExisting);
    }

    [Fact]
    public async void ReadByEmail_provided_email_exists_returns_User()
    {
        var user = await _repository.ReadByEmail("bob@email.com");

        Assert.Equal(1, user.Id);
        Assert.Equal("bob@email.com", user.Email);
        Assert.Equal("Bob", user.FirstName);
        Assert.Equal("Smith", user.LastName);
    }

    [Fact]
    public async void ReadAllAsync_returns_all_users()
    {
        var P1 = new List<int>();
        var P2 = new List<int>();

        var users = await _repository.ReadAllAsync();

        Assert.Collection(users,
            user => Assert.Equal(new UserDTO(1, "bob@email.com", "Bob", "Smith", P1), user),
            user => Assert.Equal(new UserDTO(2, "alice@email.com", "Alice", "Williams", P2), user)
        );
    }

    [Fact]
    public async void ReadAllStudentsAsync_returns_all_students()
    {
        var P1 = new List<int>();

        var users = await _repository.ReadAllAsync();

        Assert.Collection(users,
            user => Assert.Equal(new UserDTO(1, "bob@email.com", "Bob", "Smith", P1), user)
        );
    }

    [Fact]
    public async void ReadAllSupervisorsAsync_returns_all_supervisors()
    {
        var P1 = new List<int>();

        var users = await _repository.ReadAllAsync();

        Assert.Collection(users,
            user => Assert.Equal(new UserDTO(2, "alice@email.com", "Alice", "Williams", P1), user)
        );
    }

    //Add tests regarding updating student/supervisor. Requires implementation of UpdateDTO's.
    //Add tests regarding a user's projects. Requires basic implementation of Project.

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
