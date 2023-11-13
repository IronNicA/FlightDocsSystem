using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightDocsSystem.Data;
using FlightDocsSystem.Models.ManagementModels;
using FlightDocsSystem.Models.DataTransferObjectModels;
using Microsoft.Extensions.Logging; // Include the namespace for ILogger
using FlightDocsSystem.Models;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolesController> _logger; // Inject ILogger

        public RolesController(ApplicationDbContext context, ILogger<RolesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
        {
            try
            {
                var roles = await _context.Roles
                    .Select(r => new RoleDTO
                    {
                        Id = r.Id,
                        Name = r.Name,
                        NormalizedName = r.NormalizedName,
                        Creator = r.Creator
                    })
                    .ToListAsync();

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving roles");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> GetRole(int? id)
        {
            try
            {
                var role = await _context.Roles
                    .Where(r => r.Id == id)
                    .Select(r => new RoleDTO
                    {
                        Id = r.Id,
                        Name = r.Name,
                        NormalizedName = r.NormalizedName,
                        Creator = r.Creator
                    })
                    .FirstOrDefaultAsync();

                if (role == null)
                {
                    return NotFound();
                }

                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving a role");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int? id, RoleDTO roleDTO)
        {
            try
            {
                if (id != roleDTO.Id)
                {
                    return BadRequest();
                }

                var role = await _context.Roles.FindAsync(id);

                if (role == null)
                {
                    return NotFound();
                }

                // Check if the new name already exists for a different role
                if (_context.Roles.Any(r => r.Id != id && r.Name == roleDTO.Name))
                {
                    return Conflict("A role with the specified name already exists.");
                }

                // Update properties from DTO
                role.Name = roleDTO.Name;
                role.NormalizedName = roleDTO.NormalizedName;
                role.Creator = roleDTO.Creator;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a role");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<RoleDTO>> PostRole(RoleDTO roleDTO)
        {
            try
            {
                // Check if a role with the same name already exists
                if (_context.Roles.Any(r => r.Name == roleDTO.Name))
                {
                    return Conflict("A role with the same name already exists.");
                }

                var role = new Role
                {
                    Name = roleDTO.Name,
                    NormalizedName = roleDTO.NormalizedName,
                    Creator = roleDTO.Creator
                };

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetRole", new { id = role.Id }, roleDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new role");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int? id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);

                if (role == null)
                {
                    return NotFound();
                }

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a role");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }
    }
}
