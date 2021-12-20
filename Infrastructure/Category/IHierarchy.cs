public interface IHierarchy
{
    //Returns all related types of IHierarchy
    int Id{get; set;}
    IReadOnlyCollection<IHierarchy> GetAllRelated();
    bool IsRelated(IHierarchy obj);
}