using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models.ManagementModels
{
    [Table("document")]
    public class Doc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("doc_title")]
        [Required]
        public string? DocTitle { get; set; }

        [Column("flight")]
        [Required]
        public string? FlightNo { get; set; }

        [Column("doc_type")]
        [Required]
        public string? DocType { get; set; }

        [Column("doc_type_id")]
        public float? DocumentVer { get; set; }

        [Column("file_name")]
        public string? FileName { get; set; }

        [Column("create_by")]
        public string? Creator { get; set; }

        [Column("update_date")]
        public DateTime UpdateDate { get; set; } = DateTime.Now;
    }
}
