namespace ProjectBank.Infrastructure;

public class Program : CodedCategory
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
}