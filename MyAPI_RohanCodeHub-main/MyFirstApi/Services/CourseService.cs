using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Dto;
using MyFirstApi.Entities;
using MyFirstApi.IService;

namespace MyFirstApi.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses
                .AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.CourseName)
                .ToListAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CourseId == id && c.IsActive);
        }

        public async Task<Course> AddCourseAsync(CourseDto courseDto)
        {
            var course = new Course
            {
                CourseName = courseDto.CourseName,
                Description = courseDto.Description,
                DurationInMonths = courseDto.DurationInMonths,
                Fees = courseDto.Fees,
                ImageUrl = courseDto.ImageUrl,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<Course?> UpdateCourseAsync(int id, CourseDto courseDto)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseId == id && c.IsActive);

            if (course == null)
            {
                return null;
            }

            course.CourseName = courseDto.CourseName;
            course.Description = courseDto.Description;
            course.DurationInMonths = courseDto.DurationInMonths;
            course.Fees = courseDto.Fees;
            course.ImageUrl = courseDto.ImageUrl;

            await _context.SaveChangesAsync();

            return course;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseId == id && c.IsActive);

            if (course == null)
            {
                return false;
            }

            // Soft delete: course database madhun delete honar nahi.
            // Fakt inactive hoil.
            course.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}