using System.Threading.Tasks;
using FlightDocsSystem.Models.DataTransferObjectModels.User;
using FlightDocsSystem.Models.ManagementModels;

public interface IAuthenticationService
{
    Task<User> Register(UserRegisterDTO request);
    Task<string> Login(UserLoginGTO request);
    Task Logout(string userName);
}
