namespace ProjectBank.Core.EF.DTO;
public record ProgramDTO(
    int Id,

    [Required, StringLength(100)]
    string Title,

    [StringLength(1000)]
    string Description,

    [Required]
    int FacultyID,

    [Required]
    string Code,

    ICollection<int> CourseIDS

) : CodedCategoryDTO(Id, Title, Description, FacultyID, Code);

public record ProgramCreateDTO {
    public int Id {get; init;}
    [Required, StringLength(100)]
    public string Title {get; init;}

    [StringLength(1000)]
    public string Description {get; init;}

    [Required]
    public int FacultyID {get; init;}

    [Required]
    public string Code {get; init;}
    public ICollection<int> CourseIDs {get; init;}
    
    
}