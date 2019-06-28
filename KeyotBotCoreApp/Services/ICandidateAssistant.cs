using System;
namespace KeyotBotCoreApp.Services
{
    public interface ICandidateAssistant
    {
        string apikey { get; }
        string url { get; }
        string versionDate { get; }
        string workspaceId { get; }
        string username { get; }
        string password { get; }
    }
}
