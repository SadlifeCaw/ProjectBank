namespace EntityFramework.Tests;

public class BucketRepositoryTests : IDisposable
{
    private readonly ProjectBankContext _context;
    private readonly BucketRepository _repository;
    private bool disposed;

    public BucketRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();

        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();


        var Bucket1 = new ProjectBucket(new HashSet<Project>(),"Algorithm");
        var Bucket2 = new ProjectBucket(new HashSet<Project>(), "Agriculture");

        

        
        context.SaveChanges();
        _context = context;
        _repository = new BucketRepository(_context);

    }

    [Fact]
     public async void CreateAsync_creates_new_bucket_with_generated_id() 
    {
        //Arrange
        var bucketDTO = new BucketCreateDTO
        {
            ProjectIds = new HashSet<int>(),
            Key = "S3CR3TC0D3"

        };

        //Act
        var created = await _repository.CreateAsync(bucketDTO);

        if(created.Item1 == Response.Conflict)
        {
            Assert.False(true);
        }

        var i = created.Item2;

        //Assert
        Assert.Equal(Response.Created, created.Item1);
        Assert.Equal(3, i.Id);
        Assert.Equal("S3CR3TC0D3",i.Key);
        Assert.Empty(i.ProjectIds);

    }

    [Fact]
     public async void ReadByKeyAsync_provided_Key_does_not_exist_returns_Null()
    {
        var nonExisting = await _repository.ReadBucketByKeyAsync("N0TF0UND");

        Assert.Equal(null, nonExisting);
    }

    [Fact]
     public async void ReadByIDAsync_provided_ID_does_not_exist_returns_Null()
    {
        var nonExisting = await _repository.ReadBucketByIDAsync(42);

        Assert.Equal(null, nonExisting);
    }

     [Fact]
    public async void ReadAsync_provided_ID_exists_returns_Bucket()
    {
        var bucket = await _repository.ReadBucketByIDAsync(2);
        Assert.Equal(1, bucket.Id);
        Assert.Equal("Agriculture", bucket.Key);
        Assert.Empty(bucket.ProjectIds);
    }

     [Fact]
    public async void ReadAsync_provided_Key_exists_returns_Institution()
    {
        var institution = await _repository.ReadBucketByIDAsync(1);

        Assert.Equal(1, institution.Id);
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