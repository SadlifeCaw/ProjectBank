namespace ProjectBank.Core.EF.DTO;


public record InstitutionDTO(
    int Id,

    [Required, StringLength(100)]
    string Title,

    [StringLength(1000)]
    string Description

   /* [Required]
   ICollection<string> Faculties */


) : CategoryDTO(Id, Title, Description);

public record InstitutionCreateDTO
{
    [Required, StringLength(100)]
    public string Title {get; init;}

    [StringLength(1000)]
    public string Description {get; init;}
/* 
    [Required]
    public ICollection<string> Faculties {get; init;} */

}