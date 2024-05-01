using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using comNet.Data;
using comNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace comNet.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    
    public AuthenticationController(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }
    
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLoginDto login)
    {
        IActionResult response = Unauthorized();
        var user = await _userRepository.Validate(login);

        if (user == null) 
            return response;
        
        var tokenString = GenerateJSONWebToken(user);
        response = Ok(new { token = tokenString });

        return response;
    }

    private string GenerateJSONWebToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}