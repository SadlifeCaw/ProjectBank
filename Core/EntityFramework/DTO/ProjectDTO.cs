namespace ProjectBank.Core.EF.DTO;

public record ProjectKeyDTO 
{
    [Required]
    public int AuthorID {get; init;}

    [Required, StringLength(50)]
    public string Title {get; init;}
}

public record ProjectDTO(
    int Id,

    [Required]
    int AuthorID,

    [Required, StringLength(50)]
    string Title,

    [Required,StringLength(1000)]
    string Description,

    [Required]
    ProjectStatus Status,

    [Required]
    ICollection<int> TagIDs,

    [Required]
    ICollection<int> UserIDs

) : ProjectKeyDTO;

public record ProjectCreateDTO
{
    [Required]
    public int AuthorID {get; init;}

    [Required, StringLength(50)]
    public string? Title {get; init;}

    [Required,StringLength(1000)]
    public string? Description {get; init;}

    [Required]
    public ProjectStatus Status {get; init;}
    
    [Required]
    public ICollection<int> TagIDs{get; init;}

    [Required]
    public ICollection<int> UserIDs {get; init;}
};

public record ProjectUpdateDTO : ProjectCreateDTO
{
     public int Id { get; init; }
}


