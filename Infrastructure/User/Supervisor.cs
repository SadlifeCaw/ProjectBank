namespace ProjectBank.Infrastructure;

public class Supervisor : User
{
    [Required]
    public Faculty Faculty {get;set;}
}

