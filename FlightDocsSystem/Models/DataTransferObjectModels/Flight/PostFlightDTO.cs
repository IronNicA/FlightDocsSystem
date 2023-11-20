namespace FlightDocsSystem.Models.DataTransferObjectModels.Flight
{
    public class PostFlightDTO
    {
        public string? FlightNo { get; set; }

        public DateTime CreateDate { get; set; }

        public string? PoL { get; set; }

        public string? PoU { get; set; }
    }
}
