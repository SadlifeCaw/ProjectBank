namespace EntityFramework.Tests;

public class CourseRepositoryTest : IDisposable
{
    private readonly ProjectBankContext _context;
    private readonly CourseRepository _repository;
    private bool disposed;

    public CourseRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();

        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();

        var ITU = new Institution("ITU", "IT-University of Copenhagen") {Id = 1};
        var ComputerScience = new Faculty("Computer Science", "Related to computers", ITU) {Id = 2};

        TeachingProgram Software = new TeachingProgram("Software Development", "The best line at ITU", ComputerScience, "SWU2021", new List<Course>()) {Id = 3};
        TeachingProgram Data = new TeachingProgram("Data Science", "Something", ComputerScience, "DS2021", new List<Course>()) {Id = 4};
        context.Institutions.Add(ITU);
        context.Faculties.Add(ComputerScience);

        context.Programs.AddRange(
            Software,
            Data
        );

        context.Courses.AddRange(
            new Course{Id = 5, Title = "BDSA", Description = "Software Design and Architecture", Faculty = ComputerScience, Code ="BDSA2021", Programs = new[] {Software}}
        );

        context.SaveChanges();

        _context = context;
        _repository = new CourseRepository(_context);
    }

    [Fact]
    public async void CreateAsync_creates_new_course_with_generated_id() 
    {
        //Arrange
        var courseDTO = new CourseCreateDTO
        {
            Title = "DISYS",
            Description = "Distributed Systems",
            InstitutionName = "ITU",
            FacultyName = "Computer Science",
            Code = "DISYS2021",
            ProgramCodes = new List<string>() {"SWU2021"},
            StudentEmails = new List<string>(),
        };

        //Act
        var created = await _repository.CreateAsync(courseDTO);

        if(created.Item1 == Response.Conflict)
        {
            Assert.False(true);
        }

        var c = created.Item2;

        //Assert
        Assert.Equal(Response.Created, created.Item1);
        Assert.Equal(6, c.Id);
        Assert.Equal("DISYS", c.Title);
        Assert.Equal("Distributed Systems",c .Description);
        //Assert.Equal("ITU", c.); //institution name not part of basic DTO
        Assert.Equal("DISYS2021", c.Code);
        Assert.Equal(new List<string>() {"SWU2021"}, c.ProgramCodes);
    }
    
    [Fact]
    public async void ReadAllAsync_returns_all_courses()
    {
        var courses = await _repository.ReadAllAsync();
        
    }

    [Theory]

    public async void ReadByIDAsync_provided_ID_does_not_exist_returns_Null()
    {
        
    }

    [Fact]
    public async void ReadAsync_provided_ID_exists_returns_Course()
    {
        
    }



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