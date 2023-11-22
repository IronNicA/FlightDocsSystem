using FlightDocsSystem.AuthorizationAttribute;
using FlightDocsSystem.Models.DataTransferObjectModels.AppSet;
using FlightDocsSystem.Models;
using FlightDocsSystem.Service.InterfaceClass;
using FlightDocsSystem.Service.ServiceClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightDocsSystem.Data;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    [ApiController]
    [JwtAuthorization]
    public class AppSetController : ControllerBase
    {
        private readonly IAppSetService _appSetService;
        private readonly ApplicationDbContext _context;

        public AppSetController(IAppSetService appSetService, ApplicationDbContext context)
        {
            _appSetService = appSetService;
            _context = context;
        }

        [HttpPost("PostAppSet")]
        public async Task<ActionResult<AppSetResultDTO>> PostAppSet([FromForm] AppSetDTO appSetDTO)
        {
            try
            {
                var result = await _appSetService.PostAppSet(appSetDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpPut("PutTheme/{newTheme}")]
        public async Task<IActionResult> PutTheme(int newTheme)
        {
            try
            {
                var success = await _appSetService.PutTheme(newTheme);

                if (success)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpPut("UpdateLogo")]
        public async Task<IActionResult> UpdateLogo(IFormFile newFile)
        {
            try
            {
                var result = await _appSetService.UpdateLogo(newFile);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpGet("GetTheme")]
        public async Task<ActionResult<string>> GetTheme()
        {
            try
            {
                var theme = await _appSetService.GetTheme();
                return Ok(theme);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile()
        {
            try
            {
                // Retrieve the FileName from the database
                var appSet = await _context.AppSets.SingleOrDefaultAsync();

                if (appSet == null || string.IsNullOrEmpty(appSet.FileName))
                {
                    return NotFound("File not found");
                }

                // Pass the FileName to the DownloadFile method
                var result = await _appSetService.DownloadFile(appSet.FileName);
                return result;
            }
            catch (Exception ex)
            {
                // Handle errors and return an appropriate response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

