using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocsSystem.Models.ManagementModels
{
    [Table("role_permission")]
    public class RolePermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int? Id { get; set; }

        [Column("role")]
        public string? Role { get; set; }

        [Column("permission")]
        public int? Permission { get; set; }

        // Foreign key for the one-to-many relationship
        public int DocTypeId { get; set; }

        // Navigation property to represent the many side of the relationship
        public DocType? DocType { get; set; }
    }
}
