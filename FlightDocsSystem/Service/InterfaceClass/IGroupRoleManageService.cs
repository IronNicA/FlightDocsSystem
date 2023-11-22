using FlightDocsSystem.Models.DataTransferObjectModels.GroupRole;
using FlightDocsSystem.Models.DataTransferObjectModels;
using Microsoft.AspNetCore.Mvc;

public interface IGroupRoleManageService
{
    Task<IActionResult> GetAllDocTypes();
    Task<IActionResult> GetDocType(int id);
    Task<IActionResult> GetRolePermissionsByDocType(int docTypeId);
    Task<IActionResult> CreateDocType(CreateDocTypeDTO createDocTypeDTO);
    Task<IActionResult> UpdateRolePermissionPermission(int rolePermissionId, UpdatePermissionDTO updatePermissionDTO);
    IActionResult GetAllRolesAndCreators();
    IActionResult GetUsersByRole(string roleName);
    Task<IActionResult> GroupRoles();
    IActionResult GetUnAssignedUsers();
    Task<IActionResult> UnAssignUserGroup(UnAssignUserGroupDTO unAssignUserGroupDTO);
    IActionResult AssignUserRole(AssignUserRoleDTO assignUserRoleDTO);
}
