using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Dto;
using MyFirstApi.IService;

namespace MyFirstApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // Student enrolls in a course
        [HttpPost("EnrollCourse")]
        public async Task<IActionResult> EnrollCourse(
            [FromBody] EnrollmentDto enrollmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var studentId = GetLoggedInUserId();

                if (studentId == null)
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Valid student identity was not found in the token."
                    });

                var result = await _enrollmentService.EnrollCourseAsync(
                    studentId.Value,
                    enrollmentDto);

                return Ok(new
                {
                    success = true,
                    message = "Course enrollment successful.",
                    data = result
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // Logged-in student views their courses
        [HttpGet("GetMyCourses")]
        public async Task<IActionResult> GetMyCourses()
        {
            var studentId = GetLoggedInUserId();

            if (studentId == null)
                return Unauthorized(new
                {
                    success = false,
                    message = "Valid student identity was not found in the token."
                });

            var courses = await _enrollmentService.GetMyCoursesAsync(
                studentId.Value);

            return Ok(new
            {
                success = true,
                message = courses.Any()
                    ? "Enrolled courses retrieved successfully."
                    : "No enrolled courses found.",
                data = courses
            });
        }

        // Admin views all enrollments
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllEnrollments")]
        public async Task<IActionResult> GetAllEnrollments()
        {
            var enrollments =
                await _enrollmentService.GetAllEnrollmentsAsync();

            return Ok(new
            {
                success = true,
                message = enrollments.Any()
                    ? "Enrollments retrieved successfully."
                    : "No enrollments found.",
                data = enrollments
            });
        }

        private Guid? GetLoggedInUserId()
        {
            var idClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? User.FindFirst("id");

            if (idClaim == null ||
                !Guid.TryParse(idClaim.Value, out var studentId))
            {
                return null;
            }

            return studentId;
        }
    }
}