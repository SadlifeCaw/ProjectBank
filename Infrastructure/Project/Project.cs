using ProjectBank.Infrastructure.Entities;
namespace ProjectBank.Infrastructure;

[Index(nameof(Title), IsUnique = true)]

public class Project : ITagable, IProject
{

    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string? Title { get; set; }

    [Required]
    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public ProjectStatus Status { get; set; }

    [Required]
    public Category Category {get; set; }

    public IReadOnlyCollection<Tag> Tags
    {
        get{return tags;}

        set
        {
            tags = value;
            Signature = new Signature(Tags);
        }
    }

    [Required]
    private IReadOnlyCollection<Tag> tags = null!;

    [Required]
    public Signature Signature {get; set;}

    [Required]
    public ICollection<User> Users {get; set;}  = null!;

    [Required]
    public Supervisor Author {get; set;}

    [Required]
    public int MaxStudents {get; set;}

    public Project(Supervisor Author, string Title, string Description, ProjectStatus Status, Category Category, Signature Signature, IReadOnlyCollection<Tag> Tags, ICollection<User> Users, int MaxStudents)
    {
        this.Author = Author;
        this.Title = Title;
        this.Description = Description;
        this.Status = Status;
        this.Category = Category;
        this.Signature = Signature;
        this.Tags = Tags;
        this.Users = Users;
        this.MaxStudents = MaxStudents;
    }

    public Project() {}
}