namespace ProjectBank.Infrastructure;

public class Faculty: Category {
  
    [Required]
    public Institution? Institution {get; set;}
}