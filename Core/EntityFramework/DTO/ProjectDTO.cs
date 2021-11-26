namespace ProjectBank.Core.EF.DTO;

public record ProjectDTO(
    int Id,

    [Required, StringLength(50)]
    string Title,

    [Required,StringLength(1000)]
    string Description,

    [Required]
    ProjectStatus Status,

    [Required]
    ICollection<int> TagIDs
);

public record ProjectCreateDTO
{
    
}

