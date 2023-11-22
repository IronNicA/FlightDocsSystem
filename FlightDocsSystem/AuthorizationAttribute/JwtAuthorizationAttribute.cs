using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection; // Import this namespace

namespace FlightDocsSystem.AuthorizationAttribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class JwtAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly ISessionManagementService _sessionManagementService;

        public JwtAuthorizationAttribute()
        {
            // This constructor is used when applied at the controller level
        }

        public JwtAuthorizationAttribute(ISessionManagementService sessionManagementService)
        {
            _sessionManagementService = sessionManagementService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ISessionManagementService sessionManagementService = _sessionManagementService;

            if (_sessionManagementService == null)
            {
                // If _sessionManagementService is null, create a scoped instance using the service provider
                var serviceProvider = context.HttpContext.RequestServices;
                sessionManagementService = serviceProvider.GetRequiredService<ISessionManagementService>();
            }

            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token) || !sessionManagementService.IsJwtTokenActive(token))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
