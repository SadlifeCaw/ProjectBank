namespace ProjectBank.Infrastructure;

public class Course : CodedCategory 
{
    [Required]
    public ICollection<Program>? Programs {get; set;}

    [Required]
    public ICollection<Student>? Students {get; set;}
}