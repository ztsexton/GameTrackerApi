using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using GameTrackerApi.LockoutProvider;

namespace GameTrackerApi.Auth;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly SimpleLockoutProvider _lockoutProvider;

    public LoginController(IConfiguration config, SimpleLockoutProvider lockoutProvider)
    {
        _config = config;
        _lockoutProvider = lockoutProvider;
    }
    
    [HttpPost("")]
    [AllowAnonymous]
    public ActionResult Login(string apiKey)
    {
        if (_lockoutProvider.IsLockedOut() || apiKey != _config["ApiKey"])
        {
            return Unauthorized();
        }
        
        _lockoutProvider.ResetLockoutCount();
        return Ok(GenerateJsonWebToken());
    }
    
    private string GenerateJsonWebToken()    
    {    
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));    
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("username", "zsexton"),
            new Claim("team", "Washington Nationals")
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],    
            _config["Jwt:Issuer"],    
            claims,    
            expires: DateTime.Now.AddDays(120),    
            signingCredentials: credentials);    
    
        return new JwtSecurityTokenHandler().WriteToken(token);    
    }   
}