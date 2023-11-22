using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightDocsSystem.Data;
using FlightDocsSystem.Models.DataTransferObjectModels;
using FlightDocsSystem.Models.ManagementModels;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightDocsSystem.Service.ServiceClass
{
    public class RoleManageService : IRoleManageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoleManageService> _logger;

        public RoleManageService(ApplicationDbContext context, ILogger<RoleManageService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<RoleDTO>> GetRoles()
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
                throw; 
            }
        }

        public async Task<RoleDTO> GetRole(int? id)
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

                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving a role");
                throw;
            }
        }

        public async Task<IActionResult> PutRole(int? id, RoleDTO roleDTO)
        {
            try
            {
                if (id != roleDTO.Id)
                {
                    return new BadRequestResult();
                }

                var role = await _context.Roles.FindAsync(id);

                if (role == null)
                {
                    return new NotFoundResult();
                }

                if (_context.Roles.Any(r => r.Id != id && r.Name == roleDTO.Name))
                {
                    return new ConflictResult();
                }

                role.Name = roleDTO.Name;
                role.NormalizedName = roleDTO.NormalizedName;
                role.Creator = roleDTO.Creator;

                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a role");
                throw; 
            }
        }

        public async Task<ActionResult<RoleDTO>> PostRole(RoleDTO roleDTO)
        {
            try
            {
                if (_context.Roles.Any(r => r.Name == roleDTO.Name))
                {
                    return new ConflictObjectResult("A role with the same name already exists.");
                }

                var role = new Role
                {
                    Name = roleDTO.Name,
                    NormalizedName = roleDTO.NormalizedName,
                    Creator = roleDTO.Creator
                };

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                return new CreatedAtActionResult("GetRole", "Roles", new { id = role.Id }, roleDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new role");
                throw; 
            }
        }

        public async Task<IActionResult> DeleteRole(int? id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);

                if (role == null)
                {
                    return new NotFoundResult();
                }

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a role");
                throw; 
            }
        }
    }
}
