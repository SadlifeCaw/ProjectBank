namespace ProjectBank.Infrastructure;

public class CodedCategory : Category
{
    
    [Required]
    public Faculty? Faculty {get; set;}

    [Required]
    public string? Code {get; set;}
}