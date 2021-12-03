namespace ProjectBank.Infrastructure;

public abstract class CodedCategory : Category
{
    
    [Required]
    public Faculty? Faculty {get; set;}

    [Required]
    public string? Code {get; set;}

}