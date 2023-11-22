using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FlightDocsSystem.Data;
using FlightDocsSystem.Models;
using FlightDocsSystem.Models.DataTransferObjectModels.User;
using FlightDocsSystem.Models.ManagementModels;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

public class AuthenticationService : IAuthenticationService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ISessionManagementService _sessionManagementService;

    public AuthenticationService(ApplicationDbContext context, ILogger<AuthenticationService> logger, IConfiguration configuration, ISessionManagementService sessionManagementService)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
        _sessionManagementService = sessionManagementService;
    }

    public async Task<User> Register(UserRegisterDTO request)
    {
        try
        {
            if (_context.Users.Any(u => u.UserName == request.Username))
            {
                throw new Exception("Username already exists.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User
            {
                UserName = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "UnAssigned"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user registration");
            throw;
        }
    }

    public async Task<string> Login(UserLoginGTO request)
    {
        try
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == request.Username);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (user.Role == "Terminated")
            {
                throw new Exception("Login not allowed for terminated users.");
            }

            var activeSessions = _sessionManagementService.GetActiveSessions(user.UserName);
            if (activeSessions.Any())
            {
                _sessionManagementService.RemoveSession(user.UserName, activeSessions.First());
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new Exception("Wrong password.");
            }

            string token = CreateToken(user);

            _sessionManagementService.AddSession(user.UserName, token);
            _logger.LogInformation($"Active sessions for user {user.UserName}: {string.Join(", ", activeSessions)}");

            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user login");
            throw;
        }
    }

    public async Task Logout(string userName)
    {
        try
        {
            var activeSessions = _sessionManagementService.GetActiveSessions(userName);

            foreach (var session in activeSessions)
            {
                _sessionManagementService.RemoveSession(userName, session);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user logout");
            throw; 
        }
    }

    private string CreateToken(User user)
    {
        List<Claim> Claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: Claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
