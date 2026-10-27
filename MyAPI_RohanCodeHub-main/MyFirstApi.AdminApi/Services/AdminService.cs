using Microsoft.EntityFrameworkCore;
using MyFirstApi.AdminApi.Dto;
using MyFirstApi.AdminApi.Entities;
using MyFirstApi.AdminApi.IService;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;


namespace MyFirstApi.AdminApi.Services;

public class AdminService : IAdminService
{
    private readonly AdminDbContext _context;

    public AdminService(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<bool> LoginAsync(AdminLoginDto dto)
    {
        var admin = await _context.Admins
            .FirstOrDefaultAsync(x =>
                x.Email == dto.Email &&
                x.Role == "Admin");

        if (admin == null)
            return false;

        return BCrypt.Net.BCrypt.Verify(dto.Password, admin.PasswordHash);


    }
}