using MyFirstApi.AdminApi.Dto;

namespace MyFirstApi.AdminApi.IService
{
    public interface IAdminService
    {
        Task<bool> LoginAsync(AdminLoginDto dto);
    }
}