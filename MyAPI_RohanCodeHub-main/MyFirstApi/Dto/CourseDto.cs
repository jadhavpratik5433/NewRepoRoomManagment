using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Dto
{
    public class CourseDto
    {
        [Required]
        [MaxLength(150)]
        public string CourseName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Range(1, 60)]
        public int DurationInMonths { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        public decimal Fees { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }
    }
}