
namespace ProjectBank.Infrastructure;

[Index(nameof(Title), IsUnique = true)]

public class Project
{

    public int Id { get; set; }

    [Required]
    public Supervisor Author {get;}

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
    public ICollection<Student> Students {get; set;}  = null!;

    [Required]
    public ICollection<Supervisor> Collaborators {get; set;}  = null!;
}