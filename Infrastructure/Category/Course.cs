namespace ProjectBank.Infrastructure;

public class Course : CodedCategory 
{
    [Required]
    public IEnumerable<Program> Programs {get; set;}

    public override IReadOnlyCollection<IHierarchy> GetAllRelated()
    {
        var related = new List<IHierarchy>();
        related.Add(this);
        related.AddRange(Programs);
        var Faculties = Programs.Select(s => s.Faculty);
        related.AddRange(Faculties);
        related.AddRange(Faculties.Select(s => s.Institution));
        return related.AsReadOnly();
    }

    
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