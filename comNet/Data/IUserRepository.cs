using comNet.Models;

namespace comNet.Data;

public interface IUserRepository
{
    public Task AddUser(User user);
    public User? GetUser(Guid id);
    public Task<List<User>> GetUsers();
    public Task<User?> Validate(UserLoginDto loginDto);
}