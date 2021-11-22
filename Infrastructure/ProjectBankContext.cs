namespace ProjectBank.Infrastructure;

public class ProjectBankContext : DbContext
{
    //User directory
    public DbSet<User> Users { get; set; }
    public DbSet<Supervisor> Supervisors { get; set; }
    public DbSet<Student> Students { get; set; }

    //Category diretory
    public DbSet<Category> Categories { get; set; }
    public DbSet<CodedCategory> CodedCategories { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Institution> Institutions { get; set; }
    public DbSet<Program> Programs {get; set; }

    //Project directory
    public DbSet<Project> Projects { get; set; }
    public DbSet<Tag> Tags { get; set; }


    public ProjectBankContext(DbContextOptions<ProjectBankContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Save enumerator value as string
        modelBuilder
            .Entity<Project>()
            .Property(e => e.Status)
            .HasMaxLength(50)
            .HasConversion(new EnumToStringConverter<ProjectStatus>());
    }
}