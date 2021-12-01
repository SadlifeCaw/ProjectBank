/* using Xunit;

namespace EntityFramework.Tests;

public class ProgramRepositoryTest : IDisposable
{
    private readonly ProjectBankContext _context;
    private readonly ProgramRepository _repository;
    private bool disposed;

    public ProgramRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();

        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();

        List<Course> courses = new List<Course>();

        context.Courses.AddRange(
            new Course{Id = 1, Title = "Algo", Description = "algo course"},
            new Course{Id = 2, Title = "Disys", Description = "Worst course"}  
        );

        Institution institution = new Institution("ITU","Best university");
        Faculty faculty = new Faculty("Comp Sci","comp",institution);

        context.Programs.AddRange(
            new TeachingProgram("")

        context.SaveChanges();

        _context = context;
        _repository = new ProgramRepository(_context);
    }

    [Fact]
    public async void CreateAsync_creates_new_program_with_generated_id() 
    {
        //Arrange
        var programDTO = new ProgramCreateDTO
        {
            Title ="DDIT",
            Description ="Det der IT",
            FacultyName ="Not Agriculture",
            Code="DDIT2021",
            Courses = new List<Course>
        };


        //Act
        var created = await _repository.CreateAsync(programDTO);

        if(created.Item1 == Response.Conflict)
        {
            Assert.False(true);
        }

        var i = created.Item2;

        //Assert
        Assert.Equal(Response.Created, created.Item1);
        Assert.Equal(3, i.Id);
        Assert.Equal("DDIT",i.Title);
        Assert.Equal("Det der IT",i.Description);
        Assert.Equal("Not Agriculture",i.FacultyName);
        Assert.Equal("DDIT2021",i.Code);
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
} */

