using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FlightDocsSystem.Models.ManagementModels
{
    [Table("doc_type")]
    public class DocType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("doc_type_name")]
        [Required]
        public string? DocTypeName { get; set; }

        [Column("create_by")]
        [Required]
        public string? Creator { get; set; }

        [JsonIgnore]
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
