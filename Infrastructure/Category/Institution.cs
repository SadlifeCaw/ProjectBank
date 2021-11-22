namespace ProjectBank.Infrastructure;

public class Institution : Category {
    public ICollection<Faculty> Faculties {get; set;}  = null!;
}