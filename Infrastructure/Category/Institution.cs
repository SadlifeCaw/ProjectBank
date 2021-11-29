namespace ProjectBank.Infrastructure;


public class Institution : Category {

/*     [Required]
    public IEnumerable<Faculty>? Faculties {get; set;}  = null!; */

    public Institution(string Title, string Description) 
    : base(Title, Description)
    {
        
    }

    // Empty constructor to please the EF Gods
    public Institution() {}
}