using FlightDocsSystem.Data;
using FlightDocsSystem.Models.DataTransferObjectModels;
using FlightDocsSystem.Models.DataTransferObjectModels.AppSet;
using FlightDocsSystem.Models.DataTransferObjectModels.Doc;
using FlightDocsSystem.Models.ManagementModels;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace FlightDocsSystem.Service.ServiceClass
{
    public class AppSetService : IAppSetService
    {
        private readonly ILogger<AppSetService> _logger;
        private readonly ApplicationDbContext _context;

        public  AppSetService(ILogger<AppSetService> logger, ApplicationDbContext context) 
        {
            _logger = logger; 
            _context = context;
        }
        private string UploadImage(IFormFile file)
        {
            try
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Images");

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

        private void DeleteImage(string fileName)
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Images", fileName);

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

        public async Task<AppSetResultDTO> PostAppSet(AppSetDTO appSetDTO)
        {
            try
            {
                if (appSetDTO.Theme < 1 || appSetDTO.Theme > 3)
                {
                    throw new ArgumentOutOfRangeException("Theme value must be 1, 2, or 3.");
                }

                var appSet = new AppSet
                {
                    Theme = appSetDTO.Theme,
                };

                if (appSetDTO.File != null)
                {
                    appSet.FileName = UploadImage(appSetDTO.File);
                }

                _context.AppSets.Add(appSet);
                await _context.SaveChangesAsync();

                return new AppSetResultDTO
                {
                    FileName = appSet.FileName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading a file");
                throw;
            }
        }

        public async Task<bool> PutTheme(int newTheme)
        {
            try
            {
                if (newTheme < 1 || newTheme > 3)
                {
                    throw new ArgumentException("Theme value must be 1, 2, or 3.");
                }

                var appSet = await _context.AppSets.SingleOrDefaultAsync();

                if (appSet == null)
                {
                    return false;
                }

                appSet.Theme = newTheme;

                _context.Entry(appSet).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Theme");
                throw;
            }
        }

        public async Task<string> GetTheme()
        {
            try
            {
                var appSet = await _context.AppSets.SingleOrDefaultAsync();

                if (appSet == null)
                {
                    return "Default";
                }

                switch (appSet.Theme)
                {
                    case 1:
                        return "Default";
                    case 2:
                        return "Dark";
                    case 3:
                        return "Light";
                    default:
                        return "Default";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Theme");
                throw;
            }
        }
        public async Task<string> UpdateLogo(IFormFile newFile)
        {
            try
            {
                var appSet = await _context.AppSets.SingleOrDefaultAsync();

                if (appSet == null)
                {
                    return "AppSet not found";
                }

                if (newFile != null)
                {
                    DeleteImage(appSet.FileName);
                    appSet.FileName = UploadImage(newFile);
                }

                _context.Entry(appSet).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return "File updated successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating File");
                throw;
            }
        }

        public async Task<IActionResult> DownloadFile(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return new BadRequestResult();
                }

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Images", fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return new NotFoundResult();
                }

                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                // Determine the content type based on the file extension
                var contentType = GetContentType(fileName);

                var fileContentResult = new FileStreamResult(memory, contentType)
                {
                    FileDownloadName = fileName
                };

                return fileContentResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while downloading a file");
                throw;
            }
        }

        private string GetContentType(string fileName)
        {
            // Map file extensions to content types
            var contentTypeProvider = new FileExtensionContentTypeProvider();

            if (contentTypeProvider.TryGetContentType(fileName, out var contentType))
            {
                return contentType;
            }

            // Default to "application/octet-stream" if the content type is not recognized
            return "application/octet-stream";
        }

    }
}
