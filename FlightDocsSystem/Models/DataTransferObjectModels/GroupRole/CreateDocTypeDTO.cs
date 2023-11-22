namespace FlightDocsSystem.Models.DataTransferObjectModels.GroupRole
{
    public class CreateDocTypeDTO
    {
        public string? DocTypeName { get; set; }
        public List<RolePermissionDTO>? RolePermissions { get; set; }
    }
}
