using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecureSoftware.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [StringLength(100)]
        public string Username { get; set; } = null!;

        [StringLength(500)]
        public string Password { get; set; } = null!;
    }
}
