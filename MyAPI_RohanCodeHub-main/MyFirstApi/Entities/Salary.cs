using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFirstApi.Entities
{
    public class Salary
    {
        [Key]
        public Guid SalaryId { get; set; }

        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }

        public DateTime SalaryMonth { get; set; }

        public DateTime? SalaryDate { get; set; }

        public string PaymentStatus { get; set; } = "Pending";

        public string? Remarks { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Employee? Employee { get; set; }
    }
}