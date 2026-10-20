using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.Dto;
using MyFirstApi.Entities;
using MyFirstApi.IService;

namespace MyFirstApi.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly AppDbContext _context;

        public EnrollmentService(AppDbContext context)
        {
            _context = context;
        }

        // Student enroll in a course
        public async Task<Enrollment> EnrollCourseAsync(
            Guid studentId,
            EnrollmentDto enrollmentDto)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c =>
                    c.CourseId == enrollmentDto.CourseId &&
                    c.IsActive);

            if (course == null)
            {
                throw new KeyNotFoundException(
                    "Course not found or inactive.");
            }

            var alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e =>
                    e.StudentId == studentId &&
                    e.CourseId == enrollmentDto.CourseId);

            if (alreadyEnrolled)
            {
                throw new InvalidOperationException(
                    "Student is already enrolled in this course.");
            }

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = enrollmentDto.CourseId,
                EnrollmentDate = DateTime.UtcNow,
                Status = "Enrolled"
            };

            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();

            return enrollment;
        }

        // Get courses enrolled by the logged-in student
        public async Task<IEnumerable<Enrollment>> GetMyCoursesAsync(
            Guid studentId)
        {
            return await _context.Enrollments
                .AsNoTracking()
                .Include(e => e.Course)
                .Where(e =>
                    e.StudentId == studentId &&
                    e.Status == "Enrolled")
                .OrderByDescending(e => e.EnrollmentDate)
                .ToListAsync();
        }

        // Get all enrollments for Admin
        public async Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _context.Enrollments
                .AsNoTracking()
                .Include(e => e.Course)
                .OrderByDescending(e => e.EnrollmentDate)
                .ToListAsync();
        }
    }
}