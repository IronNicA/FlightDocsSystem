using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; 
using FlightDocsSystem.Data;
using FlightDocsSystem.Models.ManagementModels;
using FlightDocsSystem.Models.DataTransferObjectModels;
using FlightDocsSystem.Models;
using FlightDocsSystem.Service.ImplementClass;
using Microsoft.AspNetCore.Authorization;
using FlightDocsSystem.Service.InterfaceClass;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DocsController> _logger; 
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public DocsController(ApplicationDbContext context, ILogger<DocsController> logger, IUserService userService, IRoleService roleService)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
            _roleService = roleService;
        }


        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<DocDTO>>> GetDocs()
        {
            if (!_roleService.CanUserUseMethod(HttpContext.Request.Method))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Access Denied");
            }
            try
            {
                var docs = await _context.Docs.ToListAsync();

                var docDTOs = docs.Select(d => new DocDTO()
                {
                    Id = d.Id,
                    DocTitle = d.DocTitle,
                    DocumentVer = d.DocumentVer,
                    Creator = d.Creator,
                    CreateDate = d.CreateDate,
                    Note = d.Note,
                    FileName = d.FileName,
                });

                return Ok(docDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving documents");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<DocDTO>> GetDoc(int id)
        {
            try
            {
                var doc = await _context.Docs.FindAsync(id);

                if (doc == null)
                {
                    return NotFound();
                }

                var docDto = new DocDTO()
                {
                    Id = doc.Id,
                    DocTitle = doc.DocTitle,
                    DocumentVer = doc.DocumentVer,
                    Creator = doc.Creator,
                    CreateDate = doc.CreateDate,
                    Note = doc.Note,
                    FileName = doc.FileName
                };

                return Ok(docDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving a document");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        [HttpGet("Download/{id}"), Authorize]
        public async Task<ActionResult<DocDTO>> DownloadDoc(int id)
        {
            try
            {
                var doc = await _context.Docs.FindAsync(id);

                if (doc == null)
                {
                    return NotFound();
                }

                var docDTO = new DocDTO()
                {
                    FileName = doc.FileName
                };

                if (!string.IsNullOrEmpty(docDTO.FileName))
                {
                    string filePath = Path.Combine("Uploads", "Files", docDTO.FileName);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    return File(fileBytes, "application/octet-stream");
                }

                return Ok(docDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while downloading a document");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        [HttpPut("Update/{id}"), Authorize]
        public async Task<IActionResult> PutDoc(int id, [FromForm] DocDTO docDTO, IFormFile? file)
        {
            try
            {
                var doc = await _context.Docs.FindAsync(id);

                if (doc == null)
                {
                    return NotFound();
                }

                if (docDTO.DocTitle != null)
                {
                    doc.DocTitle = docDTO.DocTitle;
                }
                if (docDTO.DocumentVer != null)
                {
                    doc.DocumentVer = docDTO.DocumentVer;
                }
                if (docDTO.Creator != null)
                {
                    doc.Creator = docDTO.Creator;
                }
                if (docDTO.CreateDate != default)
                {
                    doc.CreateDate = docDTO.CreateDate;
                }
                if (docDTO.Note != null)
                {
                    doc.Note = docDTO.Note;
                }

                if (file != null)
                {
                    DeleteFile(doc.FileName);
                    doc.FileName = UploadFile(file);
                }

                _context.Entry(doc).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a document");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        [HttpPost("Upload_Document"), Authorize]
        public async Task<ActionResult<DocDTO>> PostDoc([FromForm] DocDTO docDTO, IFormFile file)
        {
            if (!_roleService.CanUserUseMethod(HttpContext.Request.Method))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Access Denied");
            }
            try
            {
                var doc = new Doc();
                doc.DocTitle = docDTO.DocTitle;
                doc.DocumentVer = docDTO.DocumentVer;
                doc.Creator = docDTO.Creator;
                doc.CreateDate = docDTO.CreateDate;
                doc.Note = docDTO.Note;
                doc.FileName = docDTO.FileName;

                if (file != null)
                {
                    doc.FileName = UploadFile(file);
                }

                _context.Docs.Add(doc);
                await _context.SaveChangesAsync();

                var savedDTO = new DocDTO()
                {
                    DocTitle = doc.DocTitle,
                    DocumentVer = doc.DocumentVer,
                    Creator = _userService.GetCreator(),
                    CreateDate = doc.CreateDate,
                    Note = doc.Note,
                    FileName = doc.FileName
                };

                return Ok(savedDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new document");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        [HttpDelete("Delete/{id}"), Authorize]
        public async Task<IActionResult> DeleteDoc(int id)
        {
            try
            {
                var doc = await _context.Docs.FindAsync(id);
                if (doc == null)
                {
                    return NotFound();
                }

                DeleteFile(doc.FileName);
                _context.Docs.Remove(doc);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a document");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        private string UploadFile(IFormFile file)
        {
            try
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Files");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading a file");
                throw;
            }
        }

        private void DeleteFile(string fileName)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Files", fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a file");
                throw;
            }
        }
    }
}
