    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    namespace FlightDocsSystem.Models.ManagementModels
    {
        [Table("flight")]
        public class Flight
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            [Column("Id")]
            public int Id { get; set; }

            [Column("flight_no")]
            public string? FlightNo { get; set; }

            [Column("create_by")]
            public string? Creator { get; set; }

            [Column("create_date")]
            public DateTime CreateDate { get; set; }

            [Column("point_of_loading")]
            public string? PoL { get; set; }

            [Column("point_of_unloading")]
            public string? PoU { get; set; }
        }
    }
