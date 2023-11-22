using FlightDocsSystem.AuthorizationAttribute;
using FlightDocsSystem.Models;
using FlightDocsSystem.Models.DataTransferObjectModels.User;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [JwtAuthorization]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISessionManagementService _sessionService;


        public UserController(IUserService userService, ISessionManagementService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetCurrentUserData()
        {
            try
            {
                var userDataDTO = await _userService.GetCurrentUserDataAsync();
                return Ok(userDataDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }


        [HttpPut("UpdatePhoneNumber")]
        public async Task<IActionResult> UpdatePhoneNumber([FromBody] UserPhoneUpdateDTO updateDTO)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserPhoneNumberAsync(updateDTO);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }


        [HttpGet("AllUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var userData = await _userService.GetAllUsersAsync();
                return Ok(userData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }


        [HttpGet("Active")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetActiveSessions()
        {
            try
            {
                var sessions = _sessionService.GetAllActiveSessions();
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
 

        [HttpDelete("Terminate/{userName}")]
        [Authorize(Roles = "Admin")]
        public IActionResult TerminateUser(string userName)
        {
            try
            {
                _sessionService.TerminateUserAccount(userName);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpPost("Reactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReactivateUser([FromBody] ReactivateDTO reactivateDTO)
        {
            try
            {
                return await _userService.ReactivateUser(reactivateDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
    }
}
    