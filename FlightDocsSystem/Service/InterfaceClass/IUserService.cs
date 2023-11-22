using FlightDocsSystem.Models.DataTransferObjectModels;
using FlightDocsSystem.Models.DataTransferObjectModels.User;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Service.InterfaceClass
{
    public interface IUserService
    {
        string GetCreator();
        Task<List<GetUserDTO>> GetAllUsersAsync();
        Task<IActionResult> ReactivateUser(ReactivateDTO reactivateDTO);
        Task<GetUserRequestDTO> GetCurrentUserDataAsync();
        Task<UserPhoneUpdateDTO> UpdateUserPhoneNumberAsync(UserPhoneUpdateDTO updateDTO);
    }
}
