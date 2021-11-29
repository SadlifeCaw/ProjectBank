namespace EntityFramework.Tests;

public class InstitutionRepositoryTest : IDisposable
{
    
    private readonly ProjectBankContext _context;
    private readonly InstitutionRepository _repository;
    private bool disposed;

    public InstitutionRepositoryTest()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();

        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();

        context.Institutions.AddRange(
            new Institution{Id = 1, Title = "ITU", Description = "Best university"},
            new Institution{Id = 2, Title = "KU", Description = "West university"}
        );

        context.SaveChanges();

        _context = context;
        _repository = new InstitutionRepository(_context);
    }

    [Fact]
    public async void CreateAsync_creates_new_institution_with_generated_id() 
    {
        //Arrange
        var institutionDTO = new InstitutionCreateDTO
        {
            Title = "DTU",
            Description = "Danmarks Tekniske Universitet"
        };


        //Act
        var created = await _repository.CreateAsync(institutionDTO);

        if(created.Item1 == Response.Conflict)
        {
            Assert.False(true);
        }

        var i = created.Item2;

        //Assert
        Assert.Equal(Response.Created, created.Item1);
        Assert.Equal(3, i.Id);
        Assert.Equal("DTU",i.Title);
        Assert.Equal("Danmarks Tekniske Universitet",i.Description);
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