namespace ProjectBank.Infrastructure;

public class Student : User 
{
    [Required]
    public TeachingProgram Program {get; set;}

    public Student(string Email, Institution Institution, string FirstName, string LastName, ICollection<Project> Projects, TeachingProgram Program)
    : base(Email, Institution, FirstName, LastName, Projects)
    {
        this.Program = Program;
    }

    public Student() {}

    //public ICollection<Course> Courses {get; set;} = null!;
}