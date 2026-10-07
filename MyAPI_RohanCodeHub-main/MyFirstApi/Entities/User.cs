using System.ComponentModel.DataAnnotations;

namespace MyFirstApi.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set;  }
        public string? TotpSecretKey { get; set; }
        public bool IsTotpEnabled { get; set; }
        public string? SessionId { get; set; }

    }
}
