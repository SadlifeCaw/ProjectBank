public interface IHierarchy
{
    //Returns all related types of IHierarchy
    IReadOnlyCollection<IHierarchy> GetAllRelated();
    bool IsRelated(IHierarchy obj);
}