namespace ProjectBank.Infrastructure;

public class Course : CodedCategory 
{
    public ICollection<Program>? Programs {get; set;}

    public ICollection<Student> Students {get; set;} = null!;
}