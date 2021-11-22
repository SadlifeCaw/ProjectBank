using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public abstract class Category 
{
    public int Id { get; set; }

    [Required]
    public string? Title {get; set;}
}