namespace ProjectBank.Infrastructure;

public class Faculty: Category {
    public ICollection<Institution> Institutions {get; set;}  = null!;
}