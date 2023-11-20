// FlightsController.cs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlightDocsSystem.Models;
using FlightDocsSystem.Models.DataTransferObjectModels.Flight;
using FlightDocsSystem.Service;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightsController(IFlightService flightManageService)
        {
            _flightService = flightManageService;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<FlightDTO>>> GetFlights()
        {
            try
            {
                var flights = await _flightService.GetFlights();
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<FlightDTO>> GetFlight(int id)
        {
            try
            {
                var flight = await _flightService.GetFlight(id);

                if (flight == null)
                {
                    return NotFound();
                }

                return Ok(flight);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpPut("Update/{id}"), Authorize]
        public async Task<IActionResult> PutFlight(int id, FlightDTO flightDTO)
        {
            try
            {
                var result = await _flightService.PutFlight(id, flightDTO);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpPost("AddFlight"), Authorize]
        public async Task<ActionResult<PostFlightDTO>> PostFlight(PostFlightDTO flightDTO)
        {
            try
            {
                var result = await _flightService.PostFlight(flightDTO);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }

        [HttpDelete("Delete/{id}"), Authorize]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            try
            {
                var result = await _flightService.DeleteFlight(id);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Emessage { StatusCode = StatusCodes.Status500InternalServerError, Message = ex.Message });
            }
        }
    }
}
