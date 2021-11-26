namespace ProjectBank.Core.EF.DTO;


public record InstitutionDTO(
    int Id,

    [Required, StringLength(100)]
    string Title,

    [StringLength(1000)]
    string Description,

    [Required]
    IEnumerable<int> FacultyIDs

) : CategoryDTO(Id, Title, Description);

public record InstitutionCreateDTO
{
    [Required, StringLength(100)]
    public string Title {get; init;}

    [StringLength(1000)]
    public string Description {get; init;}

    [Required]
    public IEnumerable<int>? FacultyIDs {get; init;}

}