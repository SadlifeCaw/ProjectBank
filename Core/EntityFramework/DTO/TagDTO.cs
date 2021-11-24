namespace ProjectBank.Core.EF.DTO;
public record TagDTO(
    int Id,

    [Required, StringLength(100)]
    string Name
);