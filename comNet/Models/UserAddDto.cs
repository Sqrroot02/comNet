using System.Text.Json.Serialization;

namespace comNet.Models;

public class UserAddDto
{
    [JsonPropertyName("username")]
    public string Username { get; set; }
    
    [JsonPropertyName("surname")]
    public string Surname { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("lastname")]
    public string Lastname { get; set; }
    
    [JsonPropertyName("key")]
    public string Key { get; set; }
}