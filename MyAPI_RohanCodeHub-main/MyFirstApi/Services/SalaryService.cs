using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Dto;
using MyFirstApi.Entities;
using MyFirstApi.IService;

namespace MyFirstApi.Services
{
    public class SalaryServices : ISalaryService
    {
        private readonly AppDbContext _context;

        public SalaryServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddSalary(SalaryDto dto)
        {
            try
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(x => x.Id == dto.EmployeeId);

                if (employee == null)
                {
                    return "Employee Not Found";
                }

                var salary = new Salary
                {
                    SalaryId = Guid.NewGuid(),
                    EmployeeId = dto.EmployeeId,
                    SalaryMonth = dto.SalaryMonth,
                    SalaryDate = dto.SalaryDate,
                    PaymentStatus = dto.PaymentStatus,
                    Remarks = dto.Remarks,
                    CreatedDate = DateTime.UtcNow
                };

                _context.Salaries.Add(salary);

                await _context.SaveChangesAsync();

                return "Salary Added Successfully";
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Get Salary Details By Employee Id
        public async Task<List<SalaryDto>> GetSalaryByEmployeeId(Guid employeeId)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(x => x.Id == employeeId);

            if (employee == null)
            {
                return new List<SalaryDto>();
            }

            var salaries = await _context.Salaries
                .Where(x => x.EmployeeId == employeeId)
                .Select(x => new SalaryDto
                {
                    EmployeeId = x.EmployeeId,
                    SalaryMonth = x.SalaryMonth,
                    SalaryDate = x.SalaryDate,
                    PaymentStatus = x.PaymentStatus,
                    Remarks = x.Remarks
                })
                .ToListAsync();

            return salaries;
        }
    

    }
}
