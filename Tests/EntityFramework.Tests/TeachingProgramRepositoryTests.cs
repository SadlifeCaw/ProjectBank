 using Xunit;

namespace EntityFramework.Tests;

public class TeachingProgramRepositoryTest : IDisposable
{
    private readonly ProjectBankContext _context;
    private readonly TeachingProgramRepository _repository;
    private bool disposed;

    public TeachingProgramRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();

        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();

        Institution institution = new Institution("ITU","Best university") {Id = 1};
        Faculty faculty = new Faculty("Comp Sci","comp",institution) {Id = 2};
       

        TeachingProgram software =  new TeachingProgram("SWU","Softwareudvikling",faculty,"SWU",new List<Course>()) {Id = 3};

       Course bdsa = new Course{Id = 4, Title = "BDSA", Description = "Software Design and Architecture", Faculty = faculty, Code ="BDSA2021", Programs = new[] {software}};
       Course idbs = new Course{Id = 5, Title = "IDBS", Description = "Databases", Faculty = faculty, Code ="IDBS2021", Programs = new[] {software}};

        context.Courses.AddRange(
            bdsa,
            idbs
        );

        context.Programs.AddRange(
           software =  new TeachingProgram("SWU","Softwareudvikling",faculty,"SWU",new List<Course>())
        );

        context.SaveChanges();


        _context = context;
        _repository = new TeachingProgramRepository(_context);
    }

    [Fact]
    public async void CreateAsync_creates_new_program_with_generated_id() 
    {
        //Arrange
        var programDTO = new TeachingProgramCreateDTO
        {
            Title ="DDIT",
            Description ="Det der IT",
            FacultyName ="Not Agriculture",
            Code="DDIT2021",
            CourseCodes = new List<string>()
        };



        //Act
        var created = await _repository.CreateAsync(programDTO);

        if(created.Item1 == Response.Conflict)
        {
            Assert.False(true);
        }

        var p = created.Item2;

        //Assert
        Assert.Equal(Response.Created, created.Item1);
        Assert.Equal(6, p.Id);
        Assert.Equal("DDIT",p.Title);
        Assert.Equal("Det der IT",p.Description);
        Assert.Equal("Not Agriculture",p.FacultyName);
        Assert.Equal("DDIT2021",p.Code);
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

