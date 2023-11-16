using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightDocsSystem.Data;
using FlightDocsSystem.Models.ManagementModels;
using Microsoft.AspNetCore.Authorization;
using FlightDocsSystem.Models.DataTransferObjectModels;
using Microsoft.Extensions.Logging; 
using FlightDocsSystem.Models;
using FlightDocsSystem.Service.ImplementClass;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FlightsController> _logger; 
        private readonly IUserService _userService;

        public FlightsController(ApplicationDbContext context, ILogger<FlightsController> logger, IUserService userService)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<FlightDTO>>> GetFlights()
        {
            try
            {
                var flights = await _context.Flights
                    .Select(f => new FlightDTO
                    {
                        Id = f.Id,
                        FlightNo = f.FlightNo,
                        Creator = f.Creator,
                        CreateDate = f.CreateDate,
                        PoL = f.PoL,
                        PoU = f.PoU
                    })
                    .ToListAsync();

                return flights;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving flights");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<FlightDTO>> GetFlight(int id)
        {
            try
            {
                var flight = await _context.Flights
                    .Where(f => f.Id == id)
                    .Select(f => new FlightDTO
                    {
                        Id = f.Id,
                        FlightNo = f.FlightNo,
                        Creator = f.Creator,
                        CreateDate = f.CreateDate,
                        PoL = f.PoL,
                        PoU = f.PoU
                    })
                    .FirstOrDefaultAsync();

                if (flight == null)
                {
                    return NotFound();
                }

                return flight;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving a flight");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        [HttpPut("Update/{id}"), Authorize]
        public async Task<IActionResult> PutFlight(int id, FlightDTO flightDTO)
        {
            try
            {
                if (id != flightDTO.Id)
                {
                    return BadRequest();
                }

                var flight = await _context.Flights.FindAsync(id);

                if (flight == null)
                {
                    return NotFound();
                }

                flight.FlightNo = flightDTO.FlightNo;
                flight.Creator = _userService.GetCreator();
                flight.CreateDate = flightDTO.CreateDate;
                flight.PoL = flightDTO.PoL;
                flight.PoU = flightDTO.PoU;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a flight");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        [HttpPost("AddFlight"), Authorize]
        public async Task<ActionResult<FlightDTO>> PostFlight(FlightDTO flightDTO)
        {
            try
            {
                var flight = new Flight
                {
                    FlightNo = flightDTO.FlightNo,
                    Creator = _userService.GetCreator(),
                    CreateDate = flightDTO.CreateDate,
                    PoL = flightDTO.PoL,
                    PoU = flightDTO.PoU
                };

                _context.Flights.Add(flight);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetFlight", new { id = flight.Id }, flightDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new flight");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }

        [HttpDelete("Delete/{id}"), Authorize]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            try
            {
                var flight = await _context.Flights.FindAsync(id);

                if (flight == null)
                {
                    return NotFound();
                }

                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a flight");
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = "Internal Server Error" });
            }
        }
    }
}
