using MyFirstApi.Dto;

namespace MyFirstApi.IService
{
    public interface ISalaryService
    {
        Task<string> AddSalary(SalaryDto dto);

        Task<List<SalaryDto>> GetSalaryByEmployeeId(Guid employeeId);
    }
}