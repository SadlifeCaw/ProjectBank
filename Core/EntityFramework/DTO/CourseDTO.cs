namespace ProjectBank.Core.EF.DTO;
public record CourseDTO(
    int Id,

    [Required, StringLength(100)]
    string Title,

    [StringLength(1000)]
    string Description,

    [Required]
    int FacultyID,

    [Required]
    string Code,

    ICollection<ProgramDTO> Programs,

    ICollection<StudentDTO> Students

) : CodedCategoryDTO(Id, Title, Description, FacultyID, Code);

public record CourseCreateDTO{

    [Required]
    [StringLength(100)]
    public string? Title { get; init; }

    [StringLength(1000)]
    public string?  Description{get; init;}

    [Required]
    public int FacultyID {get; init;}

    [Required]
    public string? Code {get; init;}
    public ICollection<ProgramDTO>? Programs {get; init;}

    public ICollection<StudentDTO>? Students {get; init;}
}







