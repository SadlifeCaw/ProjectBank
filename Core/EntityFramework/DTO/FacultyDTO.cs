namespace ProjectBank.Core.EF.DTO;

public record FacultyDTO(
    int Id,

    [Required, StringLength(100)]
    string Title,

    [StringLength(1000)]
    string Description,

    [Required]
    ICollection<int> InstitutionIDs

) : CategoryDTO(Id, Title, Description);

public record FacultyCreateDTO{
    // Not implemented
}
