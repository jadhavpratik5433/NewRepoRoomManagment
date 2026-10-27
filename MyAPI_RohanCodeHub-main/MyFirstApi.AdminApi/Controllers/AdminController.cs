using Microsoft.AspNetCore.Mvc;
using MyFirstApi.AdminApi.Dto;
using MyFirstApi.AdminApi.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MyFirstApi.AdminApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IConfiguration _configuration;
    public AdminController(
     IAdminService adminService,
     IConfiguration configuration)
    {
        _adminService = adminService;
        _configuration = configuration;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(AdminLoginDto dto)
    {
        var result = await _adminService.LoginAsync(dto);

        if (!result)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password"
            });
        }

        var claims = new[]
        {
        new Claim(ClaimTypes.Name, dto.Email),
        new Claim(ClaimTypes.Role, "Admin")
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["AdminJWT:Key"]!
            )
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["AdminJWT:Issuer"],
            audience: _configuration["AdminJWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );

        return Ok(new
        {
            message = "Admin login successful",
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}