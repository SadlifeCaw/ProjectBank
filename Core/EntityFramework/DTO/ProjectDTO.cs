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
    int MaxStudents,

    [Required]
    int CategoryID,

    [Required]
    ICollection<int> TagIDs,

    //not required to allow value binding in razor
    ICollection<int> UserIDs,

    [Required]
    ICollection<int> BucketIDs

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
    public int MaxStudents {get; init;}

    [Required]
    public int CategoryID {get; init;}
    
    [Required]
    public ICollection<int> TagIDs{get; init;}

    [Required]
    public ICollection<int> UserIDs {get; init;}

    [Required]
    public ICollection<int> BucketIDs {get; init;}
};

public record ProjectUpdateDTO : ProjectCreateDTO
{
     public int Id { get; init; }
}


