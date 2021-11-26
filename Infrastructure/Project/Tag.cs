namespace ProjectBank.Infrastructure;

[Index(nameof(Name), IsUnique = true)]
public class Tag
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name {get; set;}

    public Tag(string name) 
    {
        this.Name = name;
    }
}