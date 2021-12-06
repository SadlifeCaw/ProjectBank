using ProjectBank.Infrastructure.Entities;
namespace ProjectBank.Infrastructure;

[Index(nameof(Title), IsUnique = true)]

public class Project : ITagable, IProject
{

    public int Id { get; set; }

    [Required]
    public Supervisor? Author { get; init;}

    [Required]
    [StringLength(50)]
    public string? Title { get; set; }

    [Required]
    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public ProjectStatus Status { get; set; }

    [Required]
    public Category Category { get; set; }

    public IReadOnlyCollection<Tag> Tags
    {
        get{return tags;}

        set
        {
            tags = value;
            signature = new Signature(Tags);

        }
    }

    [Required]
    private IReadOnlyCollection<Tag> tags = null!;

    [Required]
    private Signature signature = null!;
    public Signature Signature
    {
        get { return signature;}
    }

    [Required]
    public ICollection<Student> Students { get; set; } = null!;

    [Required]
    public ICollection<Supervisor> Collaborators { get; set; } = null!;
}