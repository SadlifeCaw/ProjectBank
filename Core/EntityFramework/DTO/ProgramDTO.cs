namespace ProjectBank.Core.EF.DTO;
public record ProgramDTO(
    int Id,

    [Required, StringLength(100)]
    string Title,

    [StringLength(1000)]
    string Description,

    [Required]
    int FacultyID,

    [Required]
    string Code,

    ICollection<CourseDTO> Courses

) : CodedCategoryDTO(Id, Title, Description, FacultyID, Code);