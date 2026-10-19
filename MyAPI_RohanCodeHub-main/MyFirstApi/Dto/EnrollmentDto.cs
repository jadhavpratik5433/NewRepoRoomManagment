using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Dto
{
    public class EnrollmentDto
    {
        [Required]
        public int CourseId { get; set; }
    }
}