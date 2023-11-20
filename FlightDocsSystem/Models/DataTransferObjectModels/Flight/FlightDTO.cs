namespace FlightDocsSystem.Models.DataTransferObjectModels.Flight
{
    public class FlightDTO
    {
        public int Id { get; set; }

        public string? FlightNo { get; set; }

        public string? Creator { get; set; }

        public DateTime CreateDate { get; set; }

        public string? PoL { get; set; }

        public string? PoU { get; set; }
    }
}
