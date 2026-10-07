namespace MyFirstApi.Dto
{
    public class SalaryDto
    {
        public Guid EmployeeId { get; set; }

        public DateTime SalaryMonth { get; set; }

        public DateTime? SalaryDate { get; set; }

        public string PaymentStatus { get; set; } = "Pending";

        public string? Remarks { get; set; }
    }
}
