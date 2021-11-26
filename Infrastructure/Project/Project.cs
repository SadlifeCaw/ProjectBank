
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
}