
using System.Linq;
namespace ProjectBank.Infrastructure;
public abstract class Category : IHierarchy

{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? Title {get; set;}

    [StringLength(1000)]
    public string? Description {get; set;}

    public abstract IReadOnlyCollection<IHierarchy> GetAllRelated();

    public bool IsRelated(IHierarchy that)
    {
        if(that == null) throw new ArgumentException("Parameter cannot be null");
        if(GetAllRelated().Except(that.GetAllRelated()).Count() != GetAllRelated().Count()) return true;
        else return false;
    }
}