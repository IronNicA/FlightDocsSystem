using System.ComponentModel.DataAnnotations.Schema;

namespace FlightDocsSystem.Models.DataTransferObjectModels.User
{
    public class GetUserRequestDTO
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
    }
}
