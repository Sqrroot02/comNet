using comNet.Models;
using Npgsql;

namespace comNet.Data;

public class UserRepository : IUserRepository, IRepository
{
    public UserRepository()
    {
        Task.Run(async () =>
        {
            var apiAdmin = new User()
            {
                Username = "alexander.patola",
                Lastname = "Patola",
                Email = "alexander.patola@commnet.de",
                Key = "alex123",
                Surname = "Alexander"
            };
            await AddUser(apiAdmin);
        });
        
    }
    
    public async Task AddUser(User user)
    {
        if (user.Username != null && await CheckUserExists(user.Username) == 0)
        {
            var command = new NpgsqlCommand(@"INSERT INTO public.users(
	            userid, username, key, surname, email, lastname)
                VALUES(@userId, @username, @key, @surname, @email, @lastname)");
            
            command.Parameters.AddWithValue("@userId", user.UserId.ToString());
            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@key", user.Key);
            command.Parameters.AddWithValue("@surname", user.Surname);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@lastname", user.Lastname);

            await DbRequest.ExecuteCommand(command);   
        }
    }

    public async Task<int> CheckUserExists(string username)
    {
        var command = new NpgsqlCommand(@"SELECT COUNT(*) FROM public.users 
                WHERE username=@username");
        command.Parameters.AddWithValue("@username", username);

        return await DbRequest.ExecuteCount(command);
    }

    public User? GetUser(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<User>> GetUsers()
    {
        const string command = "SELECT * FROM public.users";
        var reader = await DbRequest.ExecuteQuery(command);

        var users = new List<User>();
        if (reader == null)
            return users;
        
        while (reader.Read())
        {
            var user = new User
            {
                Username = reader[1].ToString(),
                Key = reader[2].ToString(),
                Surname = reader[3].ToString(),
                Email = reader[4].ToString(),
                Lastname = reader[5].ToString()
            };
            if (Guid.TryParse(reader[0].ToString(), out var id))
                user.UserId = id;
            users.Add(user);
        }

        await reader.CloseAsync();
        return users;
    }

    public async Task<User?> Validate(UserLoginDto loginDto)
    {
        var command = new NpgsqlCommand(@"SELECT * FROM public.users 
                WHERE username=@username AND key=@key");
        command.Parameters.AddWithValue("@username", loginDto.Username);
        command.Parameters.AddWithValue("@key", loginDto.Key);

        var result = await DbRequest.ExecuteQuery(command);
        if (result is { HasRows: true })
        {
            await result.ReadAsync();
            var user = new User
            {
                Username = result[1].ToString(),
                Key = result[2].ToString(),
                Surname = result[3].ToString(),
                Email = result[4].ToString(),
                Lastname = result[5].ToString()
            };
            if (Guid.TryParse(result[0].ToString(), out var id))
                user.UserId = id;
            await result.CloseAsync();
            return user;
        }

        if (result != null)
            await result.CloseAsync();
        
        return null;
    }

    public static async Task InitTable()
    {
        await DbRequest.ExecuteCommand("CREATE TABLE users(" + 
                                       "userId      VARCHAR(256) PRIMARY KEY," +
                                       "username    VARCHAR(512)," +
                                       "key         VARCHAR(512)," +
                                       "surname     VARCHAR(256)," +
                                       "email       VARCHAR(256)," +
                                       "lastname    VARCHAR(256)" +
                                       ")");
    }
}