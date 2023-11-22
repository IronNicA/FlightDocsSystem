using System.Collections.Generic;
using System.Threading.Tasks;
using FlightDocsSystem.Models.DataTransferObjectModels;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Service.InterfaceClass
{
    public interface IRoleManageService
    {
        Task<IEnumerable<RoleDTO>> GetRoles();
        Task<RoleDTO> GetRole(int? id);
        Task<IActionResult> PutRole(int? id, RoleDTO roleDTO);
        Task<ActionResult<RoleDTO>> PostRole(RoleDTO roleDTO);
        Task<IActionResult> DeleteRole(int? id);
    }
}
