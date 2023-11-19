using FlightDocsSystem.Data;
using FlightDocsSystem.Models;
using FlightDocsSystem.Models.DataTransferObjectModels.GroupRole;
using FlightDocsSystem.Models.ManagementModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class GroupRoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DocsController> _logger;

        public GroupRoleController(ApplicationDbContext context, ILogger<DocsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetAllRolesAndCreators")]
        public IActionResult GetAllRolesAndCreators()
        {
            try
            {
                var distinctRoles = _context.Roles.Select(r => r.Name).Distinct().ToList();

                var roleInfoList = new List<RoleInfoDTO>();

                foreach (var roleName in distinctRoles)
                {
                    var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);
                    if (role != null)
                    {
                        var roleInfo = new RoleInfoDTO
                        {
                            RoleName = roleName,
                            Creator = role.Creator
                        };

                        roleInfoList.Add(roleInfo);
                    }
                }

                return Ok(roleInfoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving available groups");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpGet("GetUsersByRole/{roleName}")]
        public IActionResult GetUsersByRole(string roleName)
        {
            try
            {
                var usersWithRole = _context.Users.Where(u => u.Role == roleName).ToList();
                var usernames = usersWithRole.Select(u => u.UserName).ToList();

                var usersByRoleDTO = new UsersByRoleDTO
                {
                    Usernames = usernames
                };

                return Ok(usersByRoleDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users by role");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GroupRoles()
        {
            try
            {
                var roleNames = await _context.Roles.Select(r => r.Name).Distinct().ToListAsync();
                var roleUsersDictionary = new Dictionary<string, List<string>>();

                foreach (var roleName in roleNames)
                {
                    var usersWithRole = await _context.Users
                        .Where(u => u.Role == roleName)
                        .Select(u => u.UserName)
                        .ToListAsync();

                    roleUsersDictionary.Add(roleName, usersWithRole);
                }

                var groupRolesDTO = new GroupRolesDTO
                {
                    RoleUsers = roleUsersDictionary
                };

                return Ok(groupRolesDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users by role");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("UnAssignedUsers")]
        public IActionResult GetUnAssignedUsers()
        {
            try
            {
                var unAssignedUsers = _context.Users
                    .Where(u => u.Role == "UnAssigned")
                    .Select(u => u.UserName)
                    .ToList();

                var unAssignedUsersDto = new UnAssignedUsersDTO
                {
                    UnAssignedUsernames = unAssignedUsers
                };

                return Ok(unAssignedUsersDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users by role");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("{userId}/UnAssignGroup")]
        public async Task<IActionResult> UnAssignUserGroup([FromBody] UnAssignUserGroupDTO unAssignUserGroupDTO)
        {
            try
            {
                var userId = unAssignUserGroupDTO.UserId;
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    return NotFound($"User with ID {userId} not found.");
                }

                user.Role = "UnAssigned";
                _context.Entry(user).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok(new { Message = $"User with ID {userId} successfully unassigned." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while unassigning user from group");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("AssignUserGroupRole/{userId}/{newRole}")]
        public IActionResult AssignUserRole([FromBody] AssignUserRoleDTO assignUserRoleDTO)
        {
            try
            {
                var userId = assignUserRoleDTO.UserId;
                var newRole = assignUserRoleDTO.NewRole;

                if (string.IsNullOrEmpty(newRole))
                {
                    return BadRequest("Invalid new role.");
                }

                var user = _context.Users.Find(userId);

                if (user == null)
                {
                    return NotFound($"User with ID {userId} not found.");
                }

                if (user.Role != "UnAssigned")
                {
                    return BadRequest("User must have 'UnAssigned' role to be assigned to a new group role.");
                }

                if (newRole == "Admin")
                {
                    return BadRequest("Assignment of 'Admin' role is not allowed.");
                }

                user.Role = newRole;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(new { Message = $"Assigned role '{newRole}' to user with ID {userId}." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while assigning group role");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("UpdateRolePermission/{roleName}/{newPermission}")]
        public IActionResult UpdateRolePermission([FromBody] UpdateRolePermissionDTO updateRolePermissionDTO)
        {
            try
            {
                var roleName = updateRolePermissionDTO.RoleName;
                var newPermission = updateRolePermissionDTO.NewPermission;

                if (newPermission < 0 || newPermission > 2)
                {
                    return BadRequest("Invalid permission value. It should be 0, 1, or 2.");
                }

                var role = _context.Roles.SingleOrDefault(r => r.Name == roleName);

                if (role == null)
                {
                    return NotFound($"Role with name {roleName} not found.");
                }

                role.Permission = newPermission;
                _context.Entry(role).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(new { Message = $"Updated permission to {newPermission} for role {roleName}." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating role permission");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
