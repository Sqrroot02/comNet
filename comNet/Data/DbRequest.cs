using Npgsql;

namespace comNet.Data;

public static class DbRequest
{
    public static async Task ExecuteCommand(string command)
    {
        var connection = DbConfig.Connection;
        try
        {
            await connection.OpenAsync();
            var commandObj = new NpgsqlCommand(command, connection);
            await commandObj.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Cannot execute Sql Command... {e}");
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
    
    public static async Task<NpgsqlDataReader?> ExecuteQuery(string command)
    {
        var connection = DbConfig.Connection;
        try
        {
            await connection.OpenAsync();
            var commandObj = new NpgsqlCommand(command, connection);
            return await commandObj.ExecuteReaderAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Cannot execute Sql Query... {e}");
            await connection.CloseAsync();
        }
        return null;
    }

    public static async Task<int> ExecuteCount(NpgsqlCommand command)
    {
        var connection = DbConfig.Connection;
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            await command.PrepareAsync();
            
            var result = await command.ExecuteReaderAsync();
            await result.ReadAsync();
            return result.GetInt32(0);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Cannot execute Sql Command... {e}");
        }
        finally
        {
            await connection.CloseAsync();
        }
        return 0;
    }
    
    public static async Task<NpgsqlDataReader?> ExecuteQuery(NpgsqlCommand command)
    {
        var connection = DbConfig.Connection;
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            await command.PrepareAsync();

            return await command.ExecuteReaderAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Cannot execute Sql Command... {e}");
            await connection.CloseAsync();
        }
        return null;
    }
    
    public static async Task ExecuteCommand(NpgsqlCommand command)
    {
        var connection = DbConfig.Connection;
        try
        {
            await connection.OpenAsync();
            command.Connection = connection;
            await command.PrepareAsync();
            
            await command.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Cannot execute Sql Command... {e}");
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}