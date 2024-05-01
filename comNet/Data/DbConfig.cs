using Npgsql;

namespace comNet.Data;

public class DbConfig
{
    public static string ConnectionString = "";

    public static NpgsqlConnection Connection => new(ConnectionString);
    public static NpgsqlCommand Command(string command) => new(command, Connection);
    
    public static async Task InitDatabase()
    {
        Console.WriteLine("[DB] Build 'commnet Database'");
        await DbRequest.ExecuteCommand(@"CREATE DATABASE commnet
                                    WITH OWNER = postgres;");
        ConnectionString += "Database=commnet;";
    }
}