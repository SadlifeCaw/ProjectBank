namespace ProjectBank.Infrastructure;

public class Supervisor : User
{
    [Required]
    public Faculty Faculty {get;set;}

    public Supervisor(string Email, Institution Institution, string FirstName, string LastName, ICollection<Project> Projects, Faculty Faculty)
    : base(Email, Institution, FirstName, LastName, Projects)
    {
        this.Faculty = Faculty;
    }

    public Supervisor() {}
}

