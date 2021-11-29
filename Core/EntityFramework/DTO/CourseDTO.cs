namespace ProjectBank.Core.EF.DTO;
public record CourseDTO(
    int Id,

    [Required, StringLength(100)]
    string Title,

    [StringLength(1000)]
    string Description,

    [Required]
    string FacultyName,

    [Required]
    string Code,

    IEnumerable<string> ProgramCodes,

    IEnumerable<string> StudentEmails

) : CodedCategoryDTO(Id, Title, Description, FacultyName, Code);

public record CourseCreateDTO{

    [Required]
    [StringLength(100)]
    public string Title { get; init; }

    [StringLength(1000)]
    public string  Description{get; init;}

    [Required]
    public string FacultyName {get; init;}

    //normally this information is stored in thr faculty, but in the DTO faculty is just a string
    [Required]
    public string InstitutionName {get; init;}

    [Required]
    public string Code {get; init;}
    public ICollection<string> ProgramCodes {get; init;}

    //string is student emails
    public ICollection<string> StudentEmails {get; init;}
} 







