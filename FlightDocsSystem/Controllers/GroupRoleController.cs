using FlightDocsSystem.AuthorizationAttribute;
using FlightDocsSystem.Models;
using FlightDocsSystem.Models.DataTransferObjectModels;
using FlightDocsSystem.Models.DataTransferObjectModels.GroupRole;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [JwtAuthorization]

    public class GroupRoleApiController : ControllerBase
    {
        private readonly IGroupRoleManageService _groupRoleManageService;

        public GroupRoleApiController(IGroupRoleManageService groupRoleManageService)
        {
            _groupRoleManageService = groupRoleManageService;
        }

        [HttpGet("GetAllDocTypes")]
        public async Task<IActionResult> GetAllDocTypes()
        {
            try
            {
                var result = await _groupRoleManageService.GetAllDocTypes();

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Emessage { StatusCode = 500, Message = ex.Message });
            }
        }


        [HttpGet("doctypes/{id}")]
        public async Task<IActionResult> GetDocType(int id)
        {
            try
            {
                return await _groupRoleManageService.GetDocType(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Emessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpGet("doctypes/{docTypeId}/rolepermissions")]
        public async Task<IActionResult> GetRolePermissionsByDocType(int docTypeId)
        {
            try
            {
                return await _groupRoleManageService.GetRolePermissionsByDocType(docTypeId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Emessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpPost("doctype")]
        public async Task<IActionResult> CreateDocType([FromBody] CreateDocTypeDTO createDocTypeDTO)
        {
            try
            {
                return await _groupRoleManageService.CreateDocType(createDocTypeDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Emessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpPut("rolepermissions/{rolePermissionId}/updatepermission")]
        public async Task<IActionResult> UpdateRolePermissionPermission(int rolePermissionId, [FromBody] UpdatePermissionDTO updatePermissionDTO)
        {
            try
            {
                return await _groupRoleManageService.UpdateRolePermissionPermission(rolePermissionId, updatePermissionDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Emessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpGet("GetAllRolesAndCreators")]
        public IActionResult GetAllRolesAndCreators()
        {
            try
            {
                return _groupRoleManageService.GetAllRolesAndCreators();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Emessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpGet("GetUsersByRole/{roleName}")]
        public IActionResult GetUsersByRole(string roleName)
        {
            try
            {
                return _groupRoleManageService.GetUsersByRole(roleName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Emessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GroupRoles()
        {
            try
            {
                return await _groupRoleManageService.GroupRoles();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Emessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpGet("UnAssignedUsers")]
        public IActionResult GetUnAssignedUsers()
        {
            try
            {
                return _groupRoleManageService.GetUnAssignedUsers();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Emessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpPut("{userId}/UnAssignGroup")]
        public async Task<IActionResult> UnAssignUserGroup([FromBody] UnAssignUserGroupDTO unAssignUserGroupDTO)
        {
            try
            {
                return await _groupRoleManageService.UnAssignUserGroup(unAssignUserGroupDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Emessage { StatusCode = 500, Message = ex.Message });
            }
        }

        [HttpPut("AssignUserGroupRole/{userId}/{newRole}")]
        public IActionResult AssignUserRole([FromBody] AssignUserRoleDTO assignUserRoleDTO)
        {
            try
            {
                return _groupRoleManageService.AssignUserRole(assignUserRoleDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Emessage { StatusCode = 500, Message = ex.Message });
            }
        }
    }
}
