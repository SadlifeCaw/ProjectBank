
namespace ProjectBank.Infrastructure;
public abstract class Category 

{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string? Title {get; set;}

    [StringLength(1000)]
    public string? Description {get; set;}

    //constructor to be inherited
    protected Category(string Title, string? Description) 
    {
        this.Title = Title;
        this.Description = Description;
    }

    // Empty constructor to please the EF Gods
    protected Category() {}
}