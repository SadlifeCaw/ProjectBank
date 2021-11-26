namespace ProjectBank.Infrastructure;

public class Institution : Category {

    [Required]
    public ICollection<Faculty>? Faculties {get; set;}  = null!;

    public Institution(string Title, string Description, ICollection<Faculty> Faculties) 
    : base(Title, Description)
    {
        this.Faculties = Faculties;
    }
}