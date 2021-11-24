namespace ProjectBank.Infrastructure;

public class Institution : Category {

    [Required]
    public ICollection<Faculty> Faculties {get; set;}  = null!;
}