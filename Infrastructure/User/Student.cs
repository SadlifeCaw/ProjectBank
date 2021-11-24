namespace ProjectBank.Infrastructure;

public class Student : User 
{
    [Required]
    public int ProgramId {get; set;}

    public ICollection<Course> Courses {get; set;} = null!;
}