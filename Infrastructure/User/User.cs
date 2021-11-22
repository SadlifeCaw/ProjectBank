using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Email), IsUnique = true)]
public class User 
{

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }


    [Required]
    [StringLength(1000)]
    public string? Institution { get; set; }
    

    [Required]
    [StringLength(50)]
    public string? FirstName { get; set; }
    [Required]
    [StringLength(50)]
    public string? LastName { get; set; }
}