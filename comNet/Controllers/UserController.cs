using comNet.Data;
using comNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace comNet.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserRepository repos, ILogger<UserController> logger)
    {
        _userRepository = repos;       
        _logger = logger;
    }
    
    /// <summary>
    /// Adds a new user to DB
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public IActionResult AddUser([FromBody] UserAddDto user)
    {
        try
        {
            var newUser = new User()
            {
                Email = user.Email,
                Lastname = user.Lastname,
                Username = user.Username,
                Surname = user.Surname,
            };

            _userRepository.AddUser(newUser);
            return Created("", null);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return new StatusCodeResult(500);
        }
    }
    
    /// <summary>
    /// Returns all available users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetUsers();
        return Ok(users);
    }
}