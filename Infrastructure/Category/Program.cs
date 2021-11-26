namespace ProjectBank.Infrastructure;

public class Program : CodedCategory
{
    [Required]
    public ICollection<Course>? Courses {get; set;}

    public Program(string Title, string? Description, Faculty Faculty, string Code) 
    : base(Title, Description, Faculty, Code)
    {
        Courses = new List<Course>();
    }

    // Empty constructor to please the EF Gods
    public Program() {}
}