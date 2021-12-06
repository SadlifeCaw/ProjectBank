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

    //[Required]
    ICollection<int> StudentIDs,

    //[Required]
    ICollection<int> CollaboratorIDs

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

    [Required]
    public ISet<String> Tags{get; set;} = new HashSet<String>();

    //[Required]
    public ICollection<int> StudentIDs {get; set;}

    //[Required]
    public ICollection<int> CollaboratorIDs {get; set;}
};

public record ProjectUpdateDTO : ProjectCreateDTO
{
     public int Id { get; init; }
}