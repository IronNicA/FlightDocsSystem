using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocsSystem.Models.ManagementModels
{
    [Table("role")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int? Id { get; set; }
        [Column("role_name")]
        public string? Name { get; set; }
        [Column("role_normalized_name")]
        public string? NormalizedName { get; set; }
        [Column("role_creator")]
        public string? Creator { get; set; }
    }
}