// DocManageService.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FlightDocsSystem.Data;
using FlightDocsSystem.Models;
using FlightDocsSystem.Models.DataTransferObjectModels;
using FlightDocsSystem.Models.DataTransferObjectModels.Doc;
using FlightDocsSystem.Models.ManagementModels;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightDocsSystem.Service.ServiceClass
{
    public class DocManageService : IDocManageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DocManageService> _logger;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;

        public DocManageService(ApplicationDbContext context, ILogger<DocManageService> logger, IUserService userService, IPermissionService permissionService)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
            _permissionService = permissionService;
        }

        public async Task<IEnumerable<GetDocDTO>> GetDocs()
        {
            try
            {
                var docs = await _context.Docs.ToListAsync();

                var docDTOs = docs.Select(d => new GetDocDTO()
                {
                    Id = d.Id,
                    DocTitle = d.DocTitle,
                    DocumentVer = d.DocumentVer,
                    DocType = d.DocType,
                    Creator = d.Creator,
                    UpdateDate= d.UpdateDate,
                });

                return docDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving documents");
                throw;
            }
        }

        public async Task<GetDocDTO> GetDoc(int id, HttpContext httpContext)
        {
            try
            {
                var doc = await _context.Docs.FindAsync(id);

                if (doc == null)
                {
                    throw new KeyNotFoundException("Document not found");
                }

                var docTypeName = doc.DocType;

                if (!_permissionService.CanUserUseMethodForDocType(docTypeName, httpContext.Request.Method))
                {
                    throw new UnauthorizedAccessException("Access Denied");
                }

                var docDto = new GetDocDTO()
                {
                    DocTitle = doc.DocTitle,
                    DocumentVer = doc.DocumentVer,
                    DocType = doc.DocType,
                    Creator = doc.Creator,
                };

                return docDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving a document");
                throw;
            }
        }

        public async Task<ActionResult<DocDTO>> DownloadDoc(int id, HttpContext httpContext)
        {
            try
            {
                var doc = await _context.Docs.FindAsync(id);

                if (doc == null)
                {
                    throw new KeyNotFoundException("Document not found");
                }

                var docTypeName = doc.DocType;

                if (!_permissionService.CanUserUseMethodForDocType(docTypeName, httpContext.Request.Method))
                {
                    throw new UnauthorizedAccessException("Access Denied");
                }

                var docDTO = new DocDTO()
                {
                    FileName = doc.FileName
                };

                if (!string.IsNullOrEmpty(docDTO.FileName))
                {
                    string filePath = Path.Combine("Uploads", "Files", docDTO.FileName);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    // Determine the content type based on the file extension
                    string contentType = "application/octet-stream";
                    if (Path.GetExtension(docDTO.FileName) == ".docx")
                    {
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    }
                    else if (Path.GetExtension(docDTO.FileName) == ".doc")
                    {
                        contentType = "application/msword";
                    }

                    return new FileContentResult(fileBytes, contentType)
                    {
                        FileDownloadName = docDTO.FileName
                    };
                }

                return docDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while downloading a document");
                throw;
            }
        }


        public async Task<IActionResult> PutDoc(int id, PutDocDTO putDocDTO, IFormFile? file, HttpContext httpContext)
        {
            try
            {
                var doc = await _context.Docs.FindAsync(id);

                if (doc == null)
                {
                    throw new KeyNotFoundException("Document not found");
                }

                var docTypeName = doc.DocType;
                if (!_permissionService.CanUserUseMethodForDocType(docTypeName, httpContext.Request.Method))
                {
                    throw new UnauthorizedAccessException("Access Denied");
                }

                var newDocType = await _context.DocTypes
                    .FirstOrDefaultAsync(dt => dt.DocTypeName == putDocDTO.DocType);

                if (newDocType == null)
                {
                    throw new InvalidOperationException("Specified DocType does not exist in the database.");
                }

                if (putDocDTO.DocTitle != null)
                {
                    doc.DocTitle = putDocDTO.DocTitle;
                }

                if (putDocDTO.DocType != null)
                {
                    doc.DocType = putDocDTO.DocType;
                }

                if (file != null)
                {
                    DeleteFile(doc.FileName);
                    doc.FileName = UploadFile(file);
                }

                doc.DocumentVer += 0.1f;
                doc.UpdateDate = DateTime.UtcNow;

                _context.Entry(doc).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var updatedDocDTO = new PutDocDTO
                {
                    DocTitle = doc.DocTitle,
                    DocType = doc.DocType,
                };

                return new OkObjectResult(updatedDocDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a document");
                throw;
            }
        }


        public async Task<ActionResult<PostDocDTO>> PostDoc(PostDocDTO postDocDTO, IFormFile file)
        {
            try
            {
                var existingFlight = await _context.Flights
                    .FirstOrDefaultAsync(f => f.FlightNo == postDocDTO.FlightNo);

                if (existingFlight == null)
                {
                    throw new InvalidOperationException("Specified FlightNo does not exist in the database.");
                }

                var doc = new Doc
                {
                    DocTitle = postDocDTO.DocTitle,
                    DocumentVer = (float?)1.0,
                    DocType = postDocDTO.DocType,
                    FlightNo = postDocDTO.FlightNo,
                    Creator = _userService.GetCreator()
                };

                if (file != null)
                {
                    doc.FileName = UploadFile(file);
                }

                _context.Docs.Add(doc);
                await _context.SaveChangesAsync();

                return postDocDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new document");
                throw;
            }
        }


        public async Task<IActionResult> DeleteDoc(int id, HttpContext httpContext)
        {
            try
            {
                var doc = await _context.Docs.FindAsync(id);

                if (doc == null)
                {
                    throw new KeyNotFoundException("Document not found");
                }

                var docTypeName = doc.DocType;
                if (!_permissionService.CanUserUseMethodForDocType(docTypeName, httpContext.Request.Method))
                {
                    throw new UnauthorizedAccessException("Access Denied");
                }

                DeleteFile(doc.FileName);
                _context.Docs.Remove(doc);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a document");
                throw;
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
