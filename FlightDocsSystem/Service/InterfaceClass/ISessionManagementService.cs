using FlightDocsSystem.Models;

namespace FlightDocsSystem.Service.InterfaceClass
{
    public interface ISessionManagementService
    {
        void AddSession(string userName, string token);
        void RemoveSession(string userName, string token);
        List<string> GetActiveSessions(string userName);
        List<SessionInfo> GetAllActiveSessions();
        bool IsSessionActive(string userName, string token);
        bool IsJwtTokenActive(string token);
        void TerminateUserAccount(string userName);
    }
}
