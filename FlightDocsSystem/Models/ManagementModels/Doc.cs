using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlightDocsSystem.Models.ManagementModels
{
    [Table("doc")]
    public class Doc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("doc_title")]
        [Required]
        public string? DocTitle { get; set; }

        [Column("doc_type_id")]
        public string? DocumentVer { get; set; }

        [Column("file_name")]
        public string? FileName { get; set; }

        [Column("create_by")]
        public string? Creator { get; set; }

        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        [Column("note")]
        public string? Note { get; set; }
    }
}
