namespace ProjectBank.Infrastructure;
[Index(nameof(Email), IsUnique = true)]
public abstract class User 
{
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }


    [Required]
    [StringLength(1000)]
    public Institution? Institution { get; set; }
    

    [Required]
    [StringLength(50)]
    public string? FirstName { get; set; }
    [Required]
    [StringLength(50)]
    public string? LastName { get; set; }
}