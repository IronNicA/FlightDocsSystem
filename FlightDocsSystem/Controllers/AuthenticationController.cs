using System;
using System.Threading.Tasks;
using FlightDocsSystem.Models;
using FlightDocsSystem.Models.DataTransferObjectModels.User;
using FlightDocsSystem.Models.ManagementModels;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IUserService _userService;

    public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger, IUserService userService)
    {
        _authenticationService = authenticationService;
        _logger = logger;
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<User>> Register(UserRegisterDTO request)
    {
        try
        {
            var newUser = await _authenticationService.Register(request);

            if (newUser == null)
            {
                return BadRequest(new Emessage { StatusCode = StatusCodes.Status400BadRequest, Message = "Username already exists." });
            }

            return Ok(newUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user registration");
            return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
        }
    }

    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login(UserLoginGTO request)
    {
        try
        {
            var token = await _authenticationService.Login(request);

            if (token == null)
            {
                return BadRequest(new Emessage { StatusCode = StatusCodes.Status400BadRequest, Message = "Invalid login credentials." });
            }

            return Ok(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user login");
            return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
        }
    }

    [HttpPost("Logout"), Authorize]
    public async Task<ActionResult> Logout()
    {
        try
        {
            var userName = _userService.GetCreator();

            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest(new Emessage { StatusCode = StatusCodes.Status400BadRequest, Message = "User not authenticated!" });
            }

            await _authenticationService.Logout(userName);

            return Ok("Logout successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user logout");
            return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
        }
    }
}
