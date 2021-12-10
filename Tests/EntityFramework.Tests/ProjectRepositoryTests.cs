namespace EntityFramework.Tests;

public class ProjectRepositoryTests : IDisposable
{
    private readonly ProjectBankContext _context;
    private readonly ProjectRepository _repository;
    private bool disposed;

    public ProjectRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        builder.EnableSensitiveDataLogging();

        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();

        //ADDING PROJECTS TO REPOSITORY
        var ituInst = new Institution("ITU", "IT-Universitetet i København");
        var ituFaculty = new Faculty("Computer Science", "Computers and Science", ituInst);
        var Supervisor1 = new Supervisor("troe@itu.dk", ituInst, "Troels", "Jyde", new List<Project>(), ituFaculty, new List<Project>());
        var Tag1 = new Tag("Programming");
        var Tag2 = new Tag("Testing");

        context.Categories.AddRange(ituInst, ituFaculty);
        context.Users.AddRange(Supervisor1);
        context.Tags.AddRange(Tag1, Tag2);
        
        context.Projects.AddRange(
            new Project{Id = 1, Title = "Best Project", Description = "Simply the best project to be a part of.", Status = ProjectStatus.PUBLIC, Category = ituInst, Tags = new List<Tag>(){Tag1}, Users = new List<User>(), Buckets = new List<ProjectBucket>(), Author = Supervisor1, MaxStudents = 5},
            new Project{Id = 2, Title = "Worst Project", Description = "Don't join this project.", Status = ProjectStatus.PUBLIC, Category = ituInst, Tags = new List<Tag>(){Tag2}, Users = new List<User>(), Buckets = new List<ProjectBucket>(), Author = Supervisor1, MaxStudents = 5}
        );

        context.SaveChanges();
        _context = context;
        _repository = new ProjectRepository(_context);

    }

     [Fact]
    public async void CreateAsync_adds_a_the_new_project_to_the_repository() 
    {
        //Arrange
        var TestInstitution = new Institution("ITU", "IT-Universitetet i København");
        var TestFaculty = new Faculty("Computer Science", "Computers and Science", TestInstitution);
        var TestSupervisor = new Supervisor("troe@itu.dk", TestInstitution, "Troels", "Jyde", new List<Project>(), TestFaculty, new List<Project>());
        var project = new ProjectCreateDTO{
            AuthorID = 1,
            Title = "Test Project",
            Description = "This is a test project",
            Status = ProjectStatus.PUBLIC,
            MaxStudents = 3,
            CategoryID = 1,
            TagIDs = new List<int>(){1},
            UserIDs = new List<int>(),
            BucketIDs = new List<int>()
        };

        //Act
        var created = await _repository.CreateAsync(project);

        if(created.Item1 == Response.Conflict)
        {
            Assert.False(true);
        }

        var i = created.Item2;

        //Assert
        Assert.Equal(Response.Created, created.Item1);
        Assert.Equal(1, i.AuthorID);
        Assert.Equal(3, i.Id);
        Assert.Equal("Test Project", i.Title);
        Assert.Equal("This is a test project",i.Description);
        Assert.Equal(ProjectStatus.PUBLIC, i.Status);
        Assert.Equal(3, i.MaxStudents);
        Assert.Equal(1, i.CategoryID);
        Assert.Equal(new List<int>(){1}, i.TagIDs);
        Assert.Equal(new List<int>(), i.UserIDs);
        Assert.Equal(new List<int>(), i.BucketIDs);
    }

    
    [Fact]
    public async void ReadAllAsync_returns_all_projects()
    {

        var project1 = new ProjectDTO(1, 1, "Best Project", "Simply the best project to be a part of.", ProjectStatus.PUBLIC, 5, 2, new List<int>(){1}, new List<int>(), new List<int>());
        var project2 = new ProjectDTO(2, 1, "Worst Project", "Don't join this project.", ProjectStatus.PUBLIC, 5, 2, new List<int>(){2}, new List<int>(), new List<int>());

        var projects = await _repository.ReadAllAsync();

        Assert.Collection(projects,
           project => 
           {
                Assert.Equal(project1.Id, project.Id);
                Assert.Equal(project1.AuthorID, project.AuthorID);
                Assert.Equal(project1.Title, project.Title);
                Assert.Equal(project1.Description, project.Description);
                Assert.Equal(project1.Status, project.Status);
                Assert.Equal(project1.MaxStudents, project.MaxStudents);
                Assert.Equal(project1.CategoryID, project.CategoryID);
                Assert.Equal(project1.TagIDs, project.TagIDs);
                Assert.Equal(project1.UserIDs, project.UserIDs);
                Assert.Equal(project1.BucketIDs, project.BucketIDs);
           },
           project => 
           {
                Assert.Equal(project2.Id, project.Id);
                Assert.Equal(project2.AuthorID, project.AuthorID);
                Assert.Equal(project2.Title, project.Title);
                Assert.Equal(project2.Description, project.Description);
                Assert.Equal(project2.Status, project.Status);
                Assert.Equal(project2.MaxStudents, project.MaxStudents);
                Assert.Equal(project2.CategoryID, project.CategoryID);
                Assert.Equal(project2.TagIDs, project.TagIDs);
                Assert.Equal(project2.UserIDs, project.UserIDs);
                Assert.Equal(project2.BucketIDs, project.BucketIDs);
           }
        );
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