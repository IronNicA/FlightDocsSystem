namespace FlightDocsSystem.Models.DataTransferObjectModels.User
{
    public class GetUserDTO
    {
        public int? Id { get; set; }
        public string? Username { get; set; }

        public string? Role { get; set; }
    }
}
