namespace ProjectBank.Core.EF.DTO;

using System.Collections.Generic;

public record CategoryDTO(
   
    int Id,
    
    [Required, StringLength(100)]
    string Title,

    [StringLength(1000)]
    string Description
);

public record CodedCategoryDTO (
    int Id,

    [Required, StringLength(100)]
    string Title,

    [StringLength(1000)]
    string Description,

    [Required]
    int FacultyID,

    [Required]
    string Code

    
) : CategoryDTO(Id, Title, Description);

