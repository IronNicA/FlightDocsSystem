using FlightDocsSystem.Data; 
using FlightDocsSystem.Models;
using FlightDocsSystem.Models.DataTransferObjectModels.Role;
using FlightDocsSystem.Models.ManagementModels;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightDocsSystem.Service.ServiceClass
{
    public class SessionManagementService : ISessionManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SessionManagementService> _logger;
        private readonly IUserService _userService;
        private readonly TimeSpan _sessionTimeout = TimeSpan.FromMinutes(30); 

        public SessionManagementService(ApplicationDbContext context, ILogger<SessionManagementService> logger, IUserService userService)
        {
            _context = context;
            StartSessionCleanupTimer();
            _logger = logger;
            _userService = userService;
        }


        private void StartSessionCleanupTimer()
        {
            // Start a timer to perform session cleanup periodically
            var timer = new System.Timers.Timer
            {
                Interval = TimeSpan.FromMinutes(5).TotalMilliseconds,
                AutoReset = true,
                Enabled = true
            };

            timer.Elapsed += (sender, e) => CleanupExpiredSessions();
            timer.Start();
        }


        private void CleanupExpiredSessions()
        {
            try
            {
                var now = DateTime.Now;

            // Remove expired sessions from the database
            var expiredSessions = _context.Sessions
                .AsEnumerable()
                .Where(session => !IsSessionActive(session, now))
                .ToList();

            _context.Sessions.RemoveRange(expiredSessions);
            _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating a token");
                throw;
            }
        }


        private bool IsSessionActive(SessionInfo session, DateTime currentTime)
        {
            try
            {
                return (currentTime - session.LastAccessTime) < _sessionTimeout;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating a token");
                throw;
            }
        }


        public void AddSession(string userName, string token)
        {
            try
            {
                var session = new SessionInfo { UserName = userName, Token = token, LastAccessTime = DateTime.Now };
                _context.Sessions.Add(session);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating a token");
                throw;
            }
        }


        public void RemoveSession(string userName, string token)
        {
            try
            {
                var session = _context.Sessions.SingleOrDefault(s => s.UserName == userName && s.Token == token);
            if (session != null)
                {
                    _context.Sessions.Remove(session);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating a token");
                throw;
            }
        }


        public List<string> GetActiveSessions(string userName)
        {
            try
            {
                var activeSessions = _context.Sessions
                .Where(session => session.UserName == userName)
                .ToList()
                .Where(session => IsSessionActive(session, DateTime.Now))
                .Select(session => session.Token)
                .ToList();

            return activeSessions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating a token");
                throw;
            }
        }

        public List<SessionInfo> GetAllActiveSessions()
        {
            try
            {
                return _context.Sessions
                  .AsEnumerable()
                  .Where(s => IsSessionActive(s, DateTime.Now))
                  .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active sessions");
                throw;
            }
        }

        public bool IsSessionActive(string userName, string token)
        {
            try
            {
                var currentTime = DateTime.Now;

            var activeSession = _context.Sessions
                .AsEnumerable() 
                .Any(session => session.UserName == userName &&
                                session.Token == token &&
                                (currentTime - session.LastAccessTime) < _sessionTimeout);

            return activeSession;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating a token");
                throw;
            }
        }


        public bool IsJwtTokenActive(string token)
        {
            try
            {
                var currentTime = DateTime.Now;

                var activeSession = _context.Sessions
                    .AsEnumerable()
                    .Any(session => session.Token == token && IsSessionActive(session, currentTime));

                return activeSession;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while validating a token");
                throw;
            }
        }

        public void TerminateUserAccount(string userName)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.UserName == userName);

            if (user == null)
            {
                // User not found
                return;
            }

            // Remove any active sessions 
            var activeSessions = GetActiveSessions(userName);
            foreach (var sessionToken in activeSessions)
            {
                RemoveSession(userName, sessionToken);
            }
            user.Role = "Terminated";
            _context.SaveChanges();
        }

        public async Task<bool> TransferUserRoleAndTerminateAccount(RoleTransferDTO transferDTO)
        {
            try
            {
                var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == _userService.GetCreator());

                if (adminUser == null || adminUser.Role != "Admin")
                {
                    return false;
                }

                var userToTransfer = await _context.Users.FirstOrDefaultAsync(u => u.UserName == transferDTO.UserNameToTransfer);

                if (userToTransfer == null)
                {
                    return false;
                }

                // Transfer the role
                userToTransfer.Role = "Admin";

                // Terminate the user account
                TerminateUserAccount(adminUser.UserName);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while transferring user role and terminating account");
                throw;
            }
        }
    }
}
