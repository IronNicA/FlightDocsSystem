using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightDocsSystem.Data;
using FlightDocsSystem.Models.ManagementModels;
using Microsoft.Extensions.Logging;
using FlightDocsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using FlightDocsSystem.Service.InterfaceClass;
using FlightDocsSystem.AuthorizationAttribute;
using FlightDocsSystem.Models.DataTransferObjectModels.Role;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [JwtAuthorization]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleManageService _roleManageService;

        public RolesController(IRoleManageService roleManageService)
        {
            _roleManageService = roleManageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
        {
            try
            {
                var roles = await _roleManageService.GetRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> GetRole(int? id)
        {
            try
            {
                var role = await _roleManageService.GetRole(id);

                if (role == null)
                {
                    return NotFound();
                }

                return Ok(role);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int? id, RoleDTO roleDTO)
        {
            try
            {
                var result = await _roleManageService.PutRole(id, roleDTO);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<RoleDTO>> PostRole(RoleDTO roleDTO)
        {
            try
            {
                var result = await _roleManageService.PostRole(roleDTO);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int? id)
        {
            try
            {
                var result = await _roleManageService.DeleteRole(id);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
    }
}
