namespace ProjectBank.Infrastructure;

public class Course : CodedCategory 
{
    [Required]
    public ICollection<Program>? Programs {get; set;}

    [Required]
    public ICollection<Student>? Students {get; set;}

    public Course(string Title, string? Description, Faculty Faculty, string Code) 
    : base(Title, Description, Faculty, Code)
    {
        Programs = new List<Program>();
        Students = new List<Student>();
    }

    // Empty constructor to please the EF Gods
    public Course() {}
}