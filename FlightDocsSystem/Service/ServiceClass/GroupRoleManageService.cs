using FlightDocsSystem.Data;
using FlightDocsSystem.Models;
using FlightDocsSystem.Models.DataTransferObjectModels;
using FlightDocsSystem.Models.DataTransferObjectModels.GroupRole;
using FlightDocsSystem.Models.ManagementModels;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightDocsSystem.Service.ImplementClass
{
    public class GroupRoleManageService : IGroupRoleManageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GroupRoleManageService> _logger;
        private readonly IUserService _userService;
       

        public GroupRoleManageService(ApplicationDbContext context, ILogger<GroupRoleManageService> logger, IUserService userService)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
        }
        public async Task<IActionResult> GetAllDocTypes()
        {
            try
            {
                var docTypes = await _context.DocTypes.ToListAsync();
                return new OkObjectResult(docTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving DocType");
                throw;
            }
        }

        public async Task<IActionResult> GetDocType(int id)
        {
            try
            {
                var docType = await _context.DocTypes.FindAsync(id);

                if (docType == null)
                {
                    return new NotFoundResult();
                }

                var result = new
                {
                    Id = docType.Id,
                    DocTypeName = docType.DocTypeName,
                };

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving DocType");
                throw;
            }
        }

        public async Task<IActionResult> GetRolePermissionsByDocType(int docTypeId)
        {
            try
            {
                var docType = await _context.DocTypes.FindAsync(docTypeId);

                if (docType == null)
                {
                    return new NotFoundObjectResult($"DocType with ID {docTypeId} not found.");
                }

                var rolePermissions = await _context.RolePermissions
                    .Where(rp => rp.DocTypeId == docTypeId)
                    .ToListAsync();

                var rolePermissionDTOs = rolePermissions.Select(rp => new RolePermissionGetDTO
                {
                    Id = rp.Id,
                    Role = rp.Role,
                    Permission = rp.Permission
                }).ToList();

                return new OkObjectResult(rolePermissionDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving RolePermissions by DocType");
                throw;
            }
        }

        public async Task<IActionResult> CreateDocType(CreateDocTypeDTO createDocTypeDTO)
        {
            try
            {
                List<string> validationErrors;

                if (!IsValid(createDocTypeDTO, out validationErrors))
                {
                    return new BadRequestObjectResult(new { Errors = validationErrors });
                }
                var docType = new DocType
                    {
                        DocTypeName = createDocTypeDTO.DocTypeName,
                        Creator = _userService.GetCreator(),
                        RolePermissions = createDocTypeDTO.RolePermissions
                    .Select(rp => new RolePermission
                    {
                        Role = rp.Role,
                        Permission = rp.Permission
                    })
                    .ToList()
                    };
                _context.DocTypes.Add(docType);
                await _context.SaveChangesAsync();

                return new CreatedAtActionResult("GetDocType", "GroupRoleController", new { id = docType.Id }, docType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating DocType");
                throw;
            }
        }

        public async Task<IActionResult> UpdateRolePermissionPermission(int rolePermissionId, UpdatePermissionDTO updatePermissionDTO)
        {
            try
            {
                var rolePermission = await _context.RolePermissions.FindAsync(rolePermissionId);
                if (rolePermission == null)
                {
                    return new NotFoundObjectResult($"RolePermission with ID {rolePermissionId} not found.");
                }

                if (updatePermissionDTO.NewPermission < 0 || updatePermissionDTO.NewPermission > 2)
                {
                    return new BadRequestObjectResult("Invalid Permission value. Permission must be between 0 and 2.");
                }

                rolePermission.Permission = updatePermissionDTO.NewPermission;

                await _context.SaveChangesAsync();

                return new OkObjectResult(rolePermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating RolePermission Permission");
                throw;
            }
        }

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

                return new OkObjectResult(roleInfoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving available groups");
                throw;
            }
        }

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

                return new OkObjectResult(usersByRoleDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users by role");
                throw;
            }
        }

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

                return new OkObjectResult(groupRolesDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users by role");
                throw;
            }
        }

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

                return new OkObjectResult(unAssignedUsersDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users by role");
                throw;
            }
        }

        public async Task<IActionResult> UnAssignUserGroup(UnAssignUserGroupDTO unAssignUserGroupDTO)
        {
            try
            {
                var userId = unAssignUserGroupDTO.UserId;
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    return new NotFoundObjectResult($"User with ID {userId} not found.");
                }

                if (user.Role == "Admin")
                {
                    return new NotFoundObjectResult($"Users can not unassign a Admin");
                }

                user.Role = "UnAssigned";
                _context.Entry(user).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return new OkObjectResult(new { Message = $"User with ID {userId} successfully unassigned." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while unassigning user from group");
                throw;
            }
        }

        public IActionResult AssignUserRole(AssignUserRoleDTO assignUserRoleDTO)
        {
            try
            {
                var userId = assignUserRoleDTO.UserId;
                var newRole = assignUserRoleDTO.NewRole;

                if (string.IsNullOrEmpty(newRole))
                {
                    return new BadRequestObjectResult("Invalid new role.");
                }

                var user = _context.Users.Find(userId);

                if (user == null)
                {
                    return new NotFoundObjectResult($"User with ID {userId} not found.");
                }

                if (user.Role != "UnAssigned")
                {
                    return new BadRequestObjectResult("User must have 'UnAssigned' role to be assigned to a new group role.");
                }

                if (newRole == "Admin")
                {
                    return new BadRequestObjectResult("Assignment of 'Admin' role is not allowed.");
                }

                user.Role = newRole;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();

                return new OkObjectResult(new { Message = $"Assigned role '{newRole}' to user with ID {userId}." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while assigning group role");
                throw;
            }
        }

        private bool IsValid(CreateDocTypeDTO createDocTypeDTO, out List<string> validationErrors)
        {
            validationErrors = new List<string>();

            // Validate roles
            var existingRoleNames = _context.Roles
                .Where(r => createDocTypeDTO.RolePermissions.Select(rp => rp.Role).Contains(r.Name))
                .Select(r => r.Name)
                .ToList();

            var nonExistingRoles = createDocTypeDTO.RolePermissions
                .Where(rp => !existingRoleNames.Contains(rp.Role))
                .Select(rp => rp.Role)
                .ToList();

            // Check for restricted roles
            var restrictedRoles = nonExistingRoles.Intersect(new[] { "Admin", "UnAssigned" }).ToList();
            if (restrictedRoles.Any())
            {
                validationErrors.Add($"Cannot use restricted roles: {string.Join(", ", restrictedRoles)}");
                return false;
            }

            // Validate Permission values
            var invalidPermissions = createDocTypeDTO.RolePermissions
                .Where(rp => rp.Permission < 0 || rp.Permission > 2)
                .Select(rp => rp.Permission)
                .ToList();

            if (invalidPermissions.Any())
            {
                validationErrors.Add($"Invalid Permission values: {string.Join(", ", invalidPermissions)}");
                return false;
            }

            return true;
        }   
    }
}
