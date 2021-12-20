namespace ProjectBank.Infrastructure;


public class Institution : Category {
    /*public override IReadOnlyCollection<IHierarchy> GetAllRelated()
    {
        var related = new List<IHierarchy>();
        related.Add(this);
        return related.AsReadOnly();
    }*/

    public IEnumerable<Faculty> Faculties {get; set;}  = null!;

    public Institution(string Title, string Description) 
    : base(Title, Description)
    {

    }

    // Empty constructor to please the EF Gods
    public Institution() {}
}