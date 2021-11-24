namespace ProjectBank.Infrastructure;

[Index(nameof(Name), IsUnique = true)]
public class Tag
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string? Name {get; set;}
}