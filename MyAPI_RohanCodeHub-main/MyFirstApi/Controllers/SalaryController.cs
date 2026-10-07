using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Dto;
using MyFirstApi.GenericResponse;
using MyFirstApi.IService;
using MyFirstApi.Services;

namespace MyFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryService _salaryService;

        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }

        [HttpPost("AddSalary")]
        public async Task<IActionResult> AddSalary(
            [FromBody] SalaryDto dto)
        {
            try
            {
                var result = await _salaryService.AddSalary(dto);

                if (result == "Employee Not Found")
                {
                    return NotFound(
                        ResponseResult<string>.Failure(
                            null,
                            result
                        )
                    );
                }

                return Ok(
                    ResponseResult<string>.Success(
                        null,
                        result
                    )
                );
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("Employee/{employeeId}")]
        public async Task<IActionResult> GetSalaryByEmployeeId(Guid employeeId)
        {
            try
            {
                var result = await _salaryService.GetSalaryByEmployeeId(employeeId);

                if (result == null || result.Count == 0)
                {
                    return NotFound(
                        ResponseResult<List<SalaryDto>>.Failure(
                            null,
                            "Salary Details Not Found"
                        )
                    );
                }

                return Ok(
                    ResponseResult<List<SalaryDto>>.Success(
                        result,
                        "Salary Details Found Successfully"
                    )
                );
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}