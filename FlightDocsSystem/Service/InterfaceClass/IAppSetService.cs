using FlightDocsSystem.Models.DataTransferObjectModels.AppSet;
using FlightDocsSystem.Models.ManagementModels;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Service.InterfaceClass
{
    public interface IAppSetService
    {
        Task<AppSetResultDTO> PostAppSet(AppSetDTO appSetDTO);
        Task<bool> PutTheme(int newTheme);
        Task<string> GetTheme();
        Task<string> UpdateLogo(IFormFile newFile);
        Task<IActionResult> DownloadFile(string fileName);
    }
}
