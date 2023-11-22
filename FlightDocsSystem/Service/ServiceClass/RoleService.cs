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

        public RoleService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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
