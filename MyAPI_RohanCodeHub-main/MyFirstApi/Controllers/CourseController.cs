using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Dto;
using MyFirstApi.IService;

namespace MyFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: api/Course/GetAllCourses
        [HttpGet("GetAllCourses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        // GET: api/Course/GetCourseById/1
        [HttpGet("GetCourseById/{id:int}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);

            if (course == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Course not found."
                });
            }

            return Ok(course);
        }

        // POST: api/Course/AddCourse
        [HttpPost("AddCourse")]
        public async Task<IActionResult> AddCourse([FromBody] CourseDto courseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await _courseService.AddCourseAsync(courseDto);

            return CreatedAtAction(
                nameof(GetCourseById),
                new { id = course.CourseId },
                new
                {
                    success = true,
                    message = "Course added successfully.",
                    data = course
                });
        }

        // PUT: api/Course/UpdateCourse/1
        [HttpPut("UpdateCourse/{id:int}")]
        public async Task<IActionResult> UpdateCourse(
            int id,
            [FromBody] CourseDto courseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await _courseService.UpdateCourseAsync(id, courseDto);

            if (course == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Course not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Course updated successfully.",
                data = course
            });
        }

        // DELETE: api/Course/DeleteCourse/1
        [HttpDelete("DeleteCourse/{id:int}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var deleted = await _courseService.DeleteCourseAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Course not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Course deactivated successfully."
            });
        }
    }
}