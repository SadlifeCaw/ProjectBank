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
    [Required, StringLength(50)]
    string? Title{get; init;}

    [Required,StringLength(1000)]
    string? Description {get; init;}

    [Required]
    ProjectStatus Status{get; init;}
    
    [Required]
    ICollection<int>? TagIDs{get; init;}
}

public record ProjectUpdateDTO : ProjectCreateDTO
{
     public int Id { get; init; }
}

