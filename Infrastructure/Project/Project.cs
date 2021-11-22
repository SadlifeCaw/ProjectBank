using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

using ProjectBank.Core;

[Index(nameof(Title), IsUnique = true)]
public class Project
{

    [Required]
    [StringLength(50)]
    public string? Title {get; set;}

    [Required]
    [StringLength(1000)]
    public string? Description {get; set;}

    [Required]
    public ProjectStatus Status {get; set;}

    [Required]
    public ICollection<Tag>? Tags {get; set;}
}