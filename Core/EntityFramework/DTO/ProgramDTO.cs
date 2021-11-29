namespace ProjectBank.Core.EF.DTO;
public record ProgramDTO(
    int Id,

    [Required, StringLength(100)]
    string Title,

    [StringLength(1000)]
    string Description,

    [Required]
    string FacultyName,

    [Required]
    string Code

    //, ICollection<(string Title, string FacultyName)> Courses

) : CodedCategoryDTO(Id, Title, Description, FacultyName, Code);

public record ProgramCreateDTO {
    public int Id {get; init;}
    [Required, StringLength(100)]
    public string Title {get; init;}

    [StringLength(1000)]
    public string Description {get; init;}

    [Required]
    public string FacultyName {get; init;}

    [Required]
    public string Code {get; init;}
    public ICollection<(string Title, string FacultyName, string Code)> Courses {get; init;}
}