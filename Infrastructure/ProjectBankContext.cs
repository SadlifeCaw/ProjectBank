namespace ProjectBank.Infrastructure;

public class ProjectBankContext : DbContext
{
    //User directory
    public DbSet<User>? Users { get;}
    public DbSet<Supervisor>? Supervisors { get;}
    public DbSet<Student>? Students { get;}

    //Category diretory
    public DbSet<Category>? Categories { get;}
    public DbSet<CodedCategory>? CodedCategories { get;}
    public DbSet<Course>? Courses { get;}
    public DbSet<Faculty>? Faculties { get;}
    public DbSet<Institution>?  Institutions { get;}
    public DbSet<Program>?  Programs {get;}

    //Project directory
    public DbSet<Project>? Projects { get;}
    public DbSet<Tag>? Tags { get;}


    public ProjectBankContext(DbContextOptions<ProjectBankContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

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