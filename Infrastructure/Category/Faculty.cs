namespace ProjectBank.Infrastructure;

public class Faculty: Category {

    [Required]
    public Institution? Institution {get; set;}

    public Faculty(string Title, string Description, Institution Institution) 
    : base(Title, Description)
    {
        this.Institution = Institution;
    }

    // Empty constructor to please the EF Gods
    public Faculty() {}
}