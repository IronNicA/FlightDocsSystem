namespace FlightDocsSystem.Service.InterfaceClass
{
    public interface IRoleService
    {
        int? GetPermissionInt();
        string? GetRoleNameFromClaims();
        bool CanUserUseMethod(string httpMethod);
    }
}
