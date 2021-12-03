namespace ProjectBank.Infrastructure;

public class Faculty: Category {

    [Required]
    public Institution? Institution {get; set;}

    public override IReadOnlyCollection<IHierarchy> GetAllRelated()
    {
        var related = new List<IHierarchy>();
        related.Add(this);
        related.Add(Institution);
        return related.AsReadOnly();
    }
}