namespace ProjectBank.Infrastructure;

public class Course : CodedCategory 
{
    public ICollection<Program> Programs {get; set;}  = null!;

    public ICollection<Student> Students {get; set;} = null!;
}