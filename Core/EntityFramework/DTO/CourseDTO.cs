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
    int FacultyID {get; init;}

    [Required]
    string? Code {get; init;}
    ICollection<ProgramDTO>? Programs {get; init;}

    ICollection<StudentDTO>? Students {get; init;}
}







