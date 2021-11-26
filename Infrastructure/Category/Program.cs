namespace ProjectBank.Infrastructure;

public class Program : CodedCategory
{
    [Required]
    public ICollection<Course>? Courses {get; set;}
}