using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace FlightDocsSystem.Models.ManagementModels
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("user_name")]
        [Required]
        public string? UserName { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("password_hash")]
        public byte[]? PasswordHash { get; set; }

        [Column("password_salt")]
        public byte[]? PasswordSalt { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("role")]
        public string? Role { get; set; }
    }
}
