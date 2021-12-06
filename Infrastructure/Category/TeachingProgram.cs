namespace ProjectBank.Infrastructure;

public class TeachingProgram : CodedCategory
{
    [Required]
    public ICollection<Course>? Courses {get; set;}
    public override IReadOnlyCollection<IHierarchy> GetAllRelated()
    {
        var related = new List<IHierarchy>();
        related.Add(this);
        related.Add(Faculty);
        related.Add(Faculty.Institution);
        return related.AsReadOnly();
    }
    public TeachingProgram(string Title, string Description, Faculty Faculty, string Code, ICollection<Course> Courses) 
    : base(Title, Description, Faculty, Code)
    {
        this.Courses = Courses;
    }

    // Empty constructor to please the EF Gods
    public TeachingProgram() {}
}