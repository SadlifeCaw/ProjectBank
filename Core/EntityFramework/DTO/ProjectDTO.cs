namespace ProjectBank.Core.EF.DTO;

public record ProjectKeyDTO(int AuthorID, String Title);

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

    //[Required]
    int MaxStudents,

    //[Required]
    int CategoryID,

    //[Required]
    ICollection<int> TagIDs,

    //not required to allow value binding in razor
    ICollection<int> UserIDs,

    //[Required]
    ICollection<int> BucketIDs

);

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

    //[Required]
    public int MaxStudents {get; set;}

    //[Required]
    public int CategoryID {get; set;}
    
    public ICollection<int>? TagIDs {get; set;}

    //[Required]
    public ICollection<int>? UserIDs {get; set;}

    //[Required]
    public ICollection<int>? BucketIDs {get; set;}
};

public record ProjectUpdateDTO : ProjectCreateDTO
{
     public int Id { get; init; }
}


