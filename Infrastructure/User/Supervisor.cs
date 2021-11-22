namespace ProjectBank.Infrastructure;

public class Supervisor : User
{
    [Required]
    public Faculty FacultyId {get;set;}
}
