using FlightDocsSystem.Data;
using FlightDocsSystem.Models;
using FlightDocsSystem.Models.DataTransferObjectModels;
using FlightDocsSystem.Models.DataTransferObjectModels.User;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;

namespace FlightDocsSystem.Service.ServiceClass
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserService> _logger;
        private readonly ApplicationDbContext _context;
        public UserService(IHttpContextAccessor httpContextAccessor, ILogger<UserService> logger, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _context = context;
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

        public async Task<List<GetUserDTO>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users
                    .Select(u => new GetUserDTO
                    {
                        Id = u.Id,
                        Username = u.UserName,
                        Role = u.Role,
                    })
                    .ToListAsync();

                return users;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while retrieving roles");
                throw;
            }
        }

        public async Task<GetUserRequestDTO> GetCurrentUserDataAsync()
        {
            try
            {
                // Retrieve the username from the JWT claims
                var userName = GetCreator();

                if (string.IsNullOrEmpty(userName))
                {
                    throw new UnauthorizedAccessException("Access Denied");
                }

                // Retrieve the user data based on the username
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == userName);

                if (user == null)
                {
                    throw new KeyNotFoundException("User not found");
                }

                // Map the user data to the DTO
                var userDataDTO = new GetUserRequestDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Role = user.Role,
                };

                return userDataDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a document");
                throw;
            }
        }

        public async Task<UserPhoneUpdateDTO> UpdateUserPhoneNumberAsync(UserPhoneUpdateDTO updateDTO)
        {
            try
            {
                var userName = GetCreator();
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == userName);

                if (user == null)
                {
                    throw new KeyNotFoundException($"User with username {userName} not found.");
                }

                user.Phone= updateDTO.PhoneNumber;

                await _context.SaveChangesAsync();
                return new UserPhoneUpdateDTO
                {
                    PhoneNumber = user.Phone
                };
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, rethrow, etc.)
                _logger.LogError(ex, $"Error occurred while updating phone number : {ex.Message}");
                throw;
            }
        }

        public async Task<IActionResult> ReactivateUser([FromBody] ReactivateDTO reactivateDTO)
        {
            try
            {
                var userName = reactivateDTO.UserName;

                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == userName);

                if (user == null)
                {
                    return new NotFoundObjectResult($"User with UserName {userName} not found.");
                }

                user.Role = "UnAssigned";
                _context.Entry(user).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return new OkObjectResult(new { Message = $"User with UserName {userName} successfully reactivated." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving roles");
                throw;
            }
        }
    }
}
