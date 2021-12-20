using ProjectBank.Infrastructure.Entities;
using Microsoft.Data.Sqlite;

namespace ProjectBank.Server.Controllers;
public class TestProjectRepository
{
    private readonly HashSet<ProjectKeyDTO> _dbcontext;
    public ProjectBankContext _context{get; set;}
    public IProjectRepository TestRepository{get; set;}
    private ITagRepository _tagRepository{get; set;}
    private IUserRepository _userRepository{get; set;}
    private IBucketRepository _bucketRepository{get; set;}

    public TestProjectRepository()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();

        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();

        context.SaveChanges();
        _context = context;
        TestRepository = new ProjectRepository(_context);
        _tagRepository = new TagRepository(_context);
        _userRepository = new UserRepository(_context);
        _bucketRepository = new BucketRepository(_context);
    }
    private async void add()
    {
        for(int i = 0; i < 5; i++)
        {
            await TestRepository.CreateAsync(new ProjectCreateDTO{AuthorID = 1, Title = "Project: " + i, Description = "Description", Status = ProjectStatus.PUBLIC, MaxStudents = 3, CategoryID = 1, TagIDs = new List<int>{1}, UserIDs = new List<int>{2}, BucketIDs = new List<int>{1, 2, 3}});
        }
    }
}