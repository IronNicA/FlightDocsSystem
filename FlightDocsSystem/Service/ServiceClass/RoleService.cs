using System.Linq;
using System.Security.Claims;
using FlightDocsSystem.Data;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FlightDocsSystem.Services.ServiceClass
{
    public class RoleService : IRoleService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public RoleService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public int? GetPermissionInt()
        {
            var roleName = GetRoleNameFromClaims();

            if (!string.IsNullOrEmpty(roleName))
            {
                var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);

                if (role != null)
                {
                    return role.Permission;
                }
            }

            return null;
        }

        public bool CanUserUseMethod(string httpMethod)
        {
            int? permission = GetPermissionInt();

            if (permission.HasValue)
            {
                switch (permission.Value)
                {
                    case 0:
                        // User can't use any method
                        return false;
                    case 1:
                        // User can only use GET methods
                        return string.Equals(httpMethod, "GET", StringComparison.OrdinalIgnoreCase);
                    case 2:
                        // User can use all methods
                        return true;
                }
            }

            return false;
        }

        public string GetRoleNameFromClaims()
        {
            var roleName = string.Empty;

            if (_httpContextAccessor.HttpContext != null)
            {
                roleName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            }

            return roleName;
        }
    }
}
