using FlightDocsSystem.Data;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security;

namespace FlightDocsSystem.Service.ServiceClass
{
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PermissionService> _logger;
        private readonly IRoleService _roleService;

        public PermissionService(ApplicationDbContext context, ILogger<PermissionService> logger, IRoleService roleService)
        {
            _context = context;
            _logger = logger;
            _roleService = roleService; 
        }

        public int? GetPermissionInt(string docTypeName)
        {
            try
            {
                var roleName = _roleService.GetRoleNameFromClaims();

                if (!string.IsNullOrEmpty(roleName))
                {
                    // Check if the role is "Admin" or "Employee"
                    if (roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roleName.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                    {
                        return 2; // Set permission to 2 for Admin or Employee
                    }

                    var docType = _context.DocTypes
                        .Include(d => d.RolePermissions)
                        .FirstOrDefault(d => d.DocTypeName == docTypeName && d.RolePermissions.Any(rp => rp.Role == roleName));

                    if (docType != null)
                    {
                        var rolePermission = docType.RolePermissions.FirstOrDefault(rp => rp.Role == roleName);

                        if (rolePermission != null)
                        {
                            return rolePermission.Permission;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting permission for docTypeName: {DocTypeName}", docTypeName);
                throw; 
            }
        }


        public bool CanUserUseMethodForDocType(string docTypeName, string httpMethod)
        {
            int? permission = GetPermissionInt(docTypeName);

            if (permission.HasValue)
            {
                switch (permission.Value)
                {
                    case 0:
                        return false;
                    case 1:
                        return string.Equals(httpMethod, "GET", StringComparison.OrdinalIgnoreCase);
                    case 2:
                        return true;
                }
            }

            return false;
        }
    }
}
