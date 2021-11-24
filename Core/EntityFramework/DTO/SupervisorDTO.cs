namespace ProjectBank.Core.EF.DTO;

public record SupervisorDTO(
    int Id, 

    [Required, EmailAddress]
    string Email,

    [Required, StringLength(50)]
    string FirstName,

    [Required, StringLength(50)]
    string LastName,

    [Required]
    int FacultyID

) : UserDTO(Id, Email, FirstName, LastName);

public record SupervisorCreateDTO
{

    [EmailAddress]
    [Required]
    [StringLength(50)]
    public string? Email { get; init; }

    [Required]
    [StringLength(50)]
    public string? FirstName { get; init; }

    [Required]
    [StringLength(50)]
    public string? LastName { get; init; }

    public int? FacultyId{get; init;}
}