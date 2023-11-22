using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FlightDocsSystem.Data;
using FlightDocsSystem.Models.ManagementModels;
using FlightDocsSystem.Models.DataTransferObjectModels;
using FlightDocsSystem.Models.DataTransferObjectModels.Doc;
using Microsoft.AspNetCore.Authorization;
using FlightDocsSystem.Service.InterfaceClass;
using FlightDocsSystem.Models;
using FlightDocsSystem.Service;
using FlightDocsSystem.AuthorizationAttribute;
using FlightDocsSystem.Service.ServiceClass;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [JwtAuthorization]
    public class DocsController : ControllerBase
    {
        private readonly ILogger<DocsController> _logger;
        private readonly IDocManageService _docManageService;

        public DocsController(ILogger<DocsController> logger, IDocManageService docManageService)
        {
            _logger = logger;
            _docManageService = docManageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetDocDTO>>> GetDocs()
        {
            try
            {
                var docs = await _docManageService.GetDocs();

                return Ok(docs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving documents");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDocDTO>> GetDoc(int id)
        {
            try
            {
                var doc = await _docManageService.GetDoc(id, HttpContext);

                return Ok(doc);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving a document");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpGet("Download/{id}")]
        public async Task<ActionResult<DocDTO>> DownloadDoc(int id)
        {
            try
            {
                var doc = await _docManageService.DownloadDoc(id, HttpContext);

                return doc;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while downloading a document");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> PutDoc(int id, [FromForm] PutDocDTO putDocDTO, IFormFile? file)
        {
            try
            {
                var result = await _docManageService.PutDoc(id, putDocDTO, file, HttpContext);

                return result;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a document");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpPost("Upload_Document")]
        public async Task<ActionResult<PostDocDTO>> PostDoc([FromForm] PostDocDTO postDocDTO, IFormFile file)
        {
            try
            {
                var doc = await _docManageService.PostDoc(postDocDTO, file);

                // Return the created DTO directly
                return Ok(doc);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new document");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteDoc(int id)
        {
            try
            {
                var result = await _docManageService.DeleteDoc(id, HttpContext);

                return result;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a document");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
    }
}
