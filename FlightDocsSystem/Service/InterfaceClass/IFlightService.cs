using FlightDocsSystem.Models.DataTransferObjectModels.Flight;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Service.InterfaceClass
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightDTO>> GetFlights();
        Task<FlightDTO> GetFlight(int id);
        Task<IActionResult> PutFlight(int id, FlightDTO flightDTO);
        Task<ActionResult<PostFlightDTO>> PostFlight(PostFlightDTO flightDTO);
        Task<IActionResult> DeleteFlight(int id);
    }
}
