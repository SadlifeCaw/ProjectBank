namespace ProjectBank.Core.EF.DTO;


public record InstitutionDTO(
    int Id,

    [Required, StringLength(100)]
    string Title,

    [StringLength(1000)]
    string Description,

    [Required]
    ICollection<int> FacultyIDs

) : CategoryDTO(Id, Title, Description);