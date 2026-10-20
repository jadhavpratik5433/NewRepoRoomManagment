using MyFirstApi.Dto;
using MyFirstApi.Entities;

namespace MyFirstApi.IService
{
    public interface IEnrollmentService
    {
        Task<Enrollment> EnrollCourseAsync(
            Guid studentId,
            EnrollmentDto enrollmentDto);

        Task<IEnumerable<Enrollment>> GetMyCoursesAsync(
            Guid studentId);

        Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync();
    }
}