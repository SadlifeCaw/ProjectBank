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