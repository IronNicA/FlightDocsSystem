using FlightDocsSystem.Data;
using FlightDocsSystem.Models.DataTransferObjectModels.Flight;
using FlightDocsSystem.Models.ManagementModels;
using FlightDocsSystem.Service.ImplementClass;
using FlightDocsSystem.Service.InterfaceClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightDocsSystem.Service.ServiceClass
{
    public class FlightService : IFlightService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FlightService> _logger;
        private readonly IUserService _userService;

        public FlightService(ApplicationDbContext context, ILogger<FlightService> logger, IUserService userService)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
        }

        public async Task<IEnumerable<FlightDTO>> GetFlights()
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
                throw;
            }
        }

        public async Task<FlightDTO> GetFlight(int id)
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

                return flight;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving a flight");
                throw;
            }
        }

        public async Task<IActionResult> PutFlight(int id, FlightDTO flightDTO)
        {
            try
            {
                if (id != flightDTO.Id)
                {
                    return new BadRequestResult();
                }

                var flight = await _context.Flights.FindAsync(id);

                if (flight == null)
                {
                    return new NotFoundResult();
                }

                flight.FlightNo = flightDTO.FlightNo;
                flight.Creator = _userService.GetCreator();
                flight.CreateDate = flightDTO.CreateDate;
                flight.PoL = flightDTO.PoL;
                flight.PoU = flightDTO.PoU;

                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a flight");
                throw;
            }
        }

        public async Task<ActionResult<PostFlightDTO>> PostFlight(PostFlightDTO flightDTO)
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

                return new CreatedAtActionResult("GetFlight", "Flights", new { id = flight.Id }, flightDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new flight");
                throw;
            }
        }

        public async Task<IActionResult> DeleteFlight(int id)
        {
            try
            {
                var flight = await _context.Flights.FindAsync(id);

                if (flight == null)
                {
                    return new NotFoundResult();
                }

                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a flight");
                throw; 
            }
        }
    }
}
