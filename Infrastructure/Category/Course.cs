namespace ProjectBank.Infrastructure;

public class Course : CodedCategory 
{
    [Required]
    public IEnumerable<Program> Programs {get; set;}

    [Required]
    public IEnumerable<Student> Students {get; set;}

    public Course(string Title, string? Description, Faculty Faculty, string Code, IEnumerable<Program> Programs, IEnumerable<Student> Students) 
    : base(Title, Description, Faculty, Code)
    {
        this.Programs = Programs;
        this.Students = Students;
    }

    // Empty constructor to please the EF Gods
    public Course() {}
}