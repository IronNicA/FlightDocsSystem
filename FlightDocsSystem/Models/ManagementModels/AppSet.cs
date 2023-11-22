using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocsSystem.Models.ManagementModels
{
    [Table("app_set")]
    public class AppSet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int? Id { get; set; }

        [Column("theme_int")]
        public int? Theme { get; set; }
        [Column("logo_file")]
        public string? FileName { get; set; }
    }
}
