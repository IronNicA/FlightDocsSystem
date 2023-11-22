using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocsSystem.Models
{
    public class SessionInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("user")]
        public string? UserName { get; set; }

        [Column("token")]
        public string? Token { get; set; }

        [Column("last_access_time")]
        public DateTime LastAccessTime { get; set; }
    }
}
