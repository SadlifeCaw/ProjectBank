namespace ProjectBank.Infrastructure;

public class ProjectBankContext : DbContext
{
    //User directory

    public DbSet<User> Users => Set<User>();
    public DbSet<Supervisor> Supervisors => Set<Supervisor>();
    public DbSet<Student> Students => Set<Student>();

    //Category diretory
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CodedCategory> CodedCategories => Set<CodedCategory>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Faculty> Faculties => Set<Faculty>();
    public DbSet<Institution> Institutions => Set<Institution>();
    public DbSet<Program>  Programs => Set<Program>();

    //Project directory
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Tag> Tags => Set<Tag>();


    public ProjectBankContext(DbContextOptions<ProjectBankContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    
        // Create Table-Per-Type
        // This is slower than Table-Per-Hierarchy, but it makes database updates work

        modelBuilder.Entity<Category>().ToTable("Categories");
        modelBuilder.Entity<CodedCategory>().ToTable("CodedCategories");
        modelBuilder.Entity<Institution>().ToTable("Institutions");
        modelBuilder.Entity<Faculty>().ToTable("Faculties");
        modelBuilder.Entity<Course>().ToTable("Courses");
        modelBuilder.Entity<Program>().ToTable("Programs");

        //Save enumerator value as string
        modelBuilder
            .Entity<Project>()
            .Property(e => e.Status)
            .HasMaxLength(50)
            .HasConversion(new EnumToStringConverter<ProjectStatus>());
    }
}