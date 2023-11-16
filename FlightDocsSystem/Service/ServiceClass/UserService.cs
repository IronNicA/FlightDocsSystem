using FlightDocsSystem.Service.ImplementClass;
using System.Security.Claims;

namespace FlightDocsSystem.Service.ServiceClass
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetCreator()
        {
            var creator = string.Empty;
            if (_httpContextAccessor.HttpContext != null) 
            {
                creator = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return creator;
        }

    }
}
