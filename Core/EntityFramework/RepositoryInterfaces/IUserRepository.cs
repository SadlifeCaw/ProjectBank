namespace ProjectBank.Core.EF.Repository;

public interface IUserRepository 
{
    Task<(Response, UserDTO)> CreateAsync(UserCreateDTO user);
    Task<UserDTO> ReadByID(int userID);
    Task<IReadOnlyCollection<UserID>> ReadAllAsync();
}