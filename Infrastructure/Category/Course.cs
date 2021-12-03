namespace ProjectBank.Infrastructure;

public class Course : CodedCategory 
{
    [Required]
    public ICollection<Program>? Programs {get; set;}

    [Required]
    public ICollection<Student>? Students {get; set;}

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

    
}