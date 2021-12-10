namespace EntityFramework.Tests;

public class ProjectRepositoryTests : IDisposable
{
    private readonly ProjectBankContext _context;
    private readonly ProjectRepository _repository;

    private ProjectCreateDTO TestProject;
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

        //Creating a CreateProjectDTO() to use for test
        var TestInstitution = new Institution("ITU", "IT-Universitetet i København");
        var TestFaculty = new Faculty("Computer Science", "Computers and Science", TestInstitution);
        var TestSupervisor = new Supervisor("troe@itu.dk", TestInstitution, "Troels", "Jyde", new List<Project>(), TestFaculty, new List<Project>());
        TestProject = new ProjectCreateDTO{
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
    }

     [Fact]
    public async void CreateAsync_adds_a_the_new_project_to_the_repository() 
    {
        //Arrange

        //Act
        var created = await _repository.CreateAsync(TestProject);

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
    public async void CreateAsync_adding_exisisting_project_returns_Conflict() 
    {
        //Arrange

        //Act
        await _repository.CreateAsync(TestProject);
        var created = await _repository.CreateAsync(TestProject);
        

        var i = created.Item2;

        //Assert
        Assert.Equal(Response.Conflict, created.Item1);
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

    [Fact]
    public async void ReadAllAuthoredAsync_returns_all_projects_from_chosen_author()
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

    [Fact]
    public async void ReadAllAuthoredAsync_returns_empty_list_if_provided_authorID_does_not_exist()
    {
        Assert.True(true);
    }

    [Fact]
    public async void ReadAllByTagAsync_returns_all_projects_with_specified_tag()
    {
        Assert.True(true);
    }

    [Fact]
    public async void ReadAllByTagAsync_returns_empty_list_if_provided_tag_does_not_exist()
    {
        Assert.True(true);
    }

    [Fact]
    public async void ReadAsync_provided_ID_exists_returns_Project()
    {
        var option = await _repository.ReadByIDAsync(2);
        var project = option.Value;

        Assert.Equal(2, project.Id);
        Assert.Equal(1, project.AuthorID);
        Assert.Equal("Worst Project", project.Title);
        Assert.Equal("Don't join this project.", project.Description);
        Assert.Equal(ProjectStatus.PUBLIC, project.Status);
        Assert.Equal(5, project.MaxStudents);
        Assert.Equal(2, project.CategoryID);
        Assert.Equal(new List<int>(){2}, project.TagIDs);
        Assert.Equal(new List<int>(), project.UserIDs);
        Assert.Equal(new List<int>(), project.BucketIDs);
    }

    [Fact]
    public async void ReadByIDAsync_provided_ID_does_not_exist_returns_Null()
    {
        var nonExisting = await _repository.ReadByIDAsync(8000);

        Assert.Equal(null, nonExisting);
    }

    [Fact]
    public async void ReadByKeyAsync_returns_project_with_specified_Title_and_authorID()
    {
        Assert.True(true);
    }

    [Fact]
    public async void ReadByKeyAsync_returns_null_if_provided_Title_and_authorID_combination_does_not_exist()
    {
        Assert.True(true);
    }

    [Fact]
    public async void ReadCollectionAsync_returns_list_with_projects_from_provided_projectIDs()
    {
        Assert.True(true);
    }

    [Fact]
    public async void ReadCollectionAsync_returns_emptylist_if_the_content_of_provided_list_of_IDs_does_not_exist()
    {
        Assert.True(true);
    }

    [Fact]
    public async void AddUserAsync_adds_specified_user_to_specified_project()
    {
        Assert.True(true);
        //Test også response.updated
    }

    [Fact]
    public async void AddUserAsync_returns_NotFound_if_provided_userID_or_ProjectKey_does_not_exist()
    {
        Assert.True(true);
    }  

    [Fact]
    public async void RemoveUserAsync_removes_specified_user_from_specified_project()
    {
        Assert.True(true);
        //Test også response.updated
    }

    [Fact]
    public async void RemoveUserAsync_returns_NotFound_if_provided_userID_or_ProjectKey_does_not_exist()
    {
        Assert.True(true);
    }   

    [Fact]
    public async void UpdateAsync_updates_project_succesfully_given_that_provided_ProjectUpdateDTO_is_valid()
    {
        Assert.True(true);
        //Test også response.updated
    }

    [Fact]
    public async void UpdateAsync_returns_NotFound_if_project_does_not_exist()
    {
        Assert.True(true);
    } 

    [Fact]
    public async void UpdateAsync_returns_NotFound_if_provided_category_does_not_exist()
    {
        Assert.True(true);
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