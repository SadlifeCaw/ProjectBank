
namespace ProjectBank.Infrastructure;

[Index(nameof(Title), IsUnique = true)]

public class Project
{

    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string? Title {get; set;}

    [Required]
    [StringLength(1000)]
    public string? Description {get; set;}

    [Required]
    public ProjectStatus Status {get; set;}

    [Required]
    public ICollection<Tag> Tags {get; set;}  = null!;

    [Required]
    public ICollection<User> Users {get; set;}  = null!;

    [Required]
    public Supervisor Author {get; set;}

    [Required]
    public int MaxStudents {get; set;}

    public Project(Supervisor Author, string Title, string Description, ProjectStatus Status, ICollection<Tag> Tags, ICollection<User> Users)
    {
        this.Author = Author;
        this.Title = Title;
        this.Description = Description;
        this.Status = Status;
        this.Tags = Tags;
        this.Users = Users;
    }

    public Project() {}
}