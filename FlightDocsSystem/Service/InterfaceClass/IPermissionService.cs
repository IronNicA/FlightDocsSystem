namespace FlightDocsSystem.Service.InterfaceClass
{
    public interface IPermissionService
    {
        int? GetPermissionInt(string docTypeName);
        bool CanUserUseMethodForDocType(string docTypeName, string httpMethod);
    }
}
