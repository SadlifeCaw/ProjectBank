namespace ProjectBank.Infrastructure;

public class Student : User 
{
    [Required]
    public Program? ProgramId {get; set;}

    public ICollection<Course> Courses {get; set;} = null!;

}