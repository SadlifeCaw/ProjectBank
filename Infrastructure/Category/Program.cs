namespace ProjectBank.Infrastructure;

public class Program : CodedCategory
{
    public ICollection<Course> Courses {get; set;}  = null!;
}