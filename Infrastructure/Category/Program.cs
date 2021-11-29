namespace ProjectBank.Infrastructure;

public class Program : CodedCategory
{
    [Required]
    public ICollection<Course>? Courses {get; set;}

    public Program(string Title, string? Description, Faculty Faculty, string Code, ICollection<Course> Courses) 
    : base(Title, Description, Faculty, Code)
    {
        this.Courses = Courses;
    }

    // Empty constructor to please the EF Gods
    public Program() {}
}