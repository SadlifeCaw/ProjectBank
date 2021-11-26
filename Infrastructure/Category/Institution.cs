namespace ProjectBank.Infrastructure;

public class Institution : Category {

    [Required]
    public IEnumerable<Faculty>? Faculties {get; set;}  = null!;

    public Institution(string Title, string Description, IEnumerable<Faculty> Faculties) 
    : base(Title, Description)
    {
        this.Faculties = Faculties;
    }

    // Empty constructor to please the EF Gods
    public Institution() {}
}