using System.Collections.Generic;
using System.Threading.Tasks;
using FlightDocsSystem.Models.DataTransferObjectModels.Doc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Service
{
    public interface IDocManageService
    {
        Task<IEnumerable<GetDocDTO>> GetDocs();
        Task<GetDocDTO> GetDoc(int id, HttpContext httpContext);
        Task<ActionResult<DocDTO>> DownloadDoc(int id, HttpContext httpContext);
        Task<IActionResult> PutDoc(int id, PutDocDTO putDocDTO, IFormFile? file, HttpContext httpContext);
        Task<ActionResult<PostDocDTO>> PostDoc(PostDocDTO postDocDTO, IFormFile file);
        Task<IActionResult> DeleteDoc(int id, HttpContext httpContext);
    }
}
