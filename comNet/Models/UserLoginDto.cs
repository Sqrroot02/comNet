using System.Text.Json.Serialization;

namespace comNet.Models;

public class UserLoginDto
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("key")]
    public string? Key { get; set; }
}