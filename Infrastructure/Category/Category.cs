
namespace ProjectBank.Infrastructure;
public abstract class Category 

{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? Title {get; set;}

    [StringLength(1000)]
    public string? Description {get; set;}
}