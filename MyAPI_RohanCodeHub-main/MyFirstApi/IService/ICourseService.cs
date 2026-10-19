using MyFirstApi.Dto;
using MyFirstApi.Entities;

namespace MyFirstApi.IService
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();

        Task<Course?> GetCourseByIdAsync(int id);

        Task<Course> AddCourseAsync(CourseDto courseDto);

        Task<Course?> UpdateCourseAsync(int id, CourseDto courseDto);

        Task<bool> DeleteCourseAsync(int id);
    }
}