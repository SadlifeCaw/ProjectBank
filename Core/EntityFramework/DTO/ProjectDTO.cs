namespace ProjectBank.Core.EF.DTO;

public record ProjectKeyDTO
{
    [Required]
    public int AuthorID {get; init;}

    [Required, StringLength(50)]
    public string Title{get; init;}
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

    ICollection<int> TagIDs,

    ICollection<int> UserIDs

) : ProjectKeyDTO;

public record ProjectCreateDTO
{
    [Required]
    public int AuthorID {get; init;}

    [Required, StringLength(50)]
    public string? Title {get; set;}

    [Required,StringLength(1000)]
    public string? Description {get; set;}

    [Required]
    public ProjectStatus Status {get; set;}
    
    public ICollection<int> TagIDs {get; set;}

    public ICollection<int> UserIDs {get; set;}
};

public record ProjectUpdateDTO : ProjectCreateDTO
{
     public int Id { get; init; }
}


