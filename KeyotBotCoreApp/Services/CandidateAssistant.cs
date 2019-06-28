using System;
namespace KeyotBotCoreApp.Services
{
    public class CandidateAssistant: ICandidateAssistant
    {
        public CandidateAssistant()
        {
        }

        string ICandidateAssistant.apikey => throw new NotImplementedException();

        string ICandidateAssistant.url => throw new NotImplementedException();

        string ICandidateAssistant.versionDate => throw new NotImplementedException();

        string ICandidateAssistant.workspaceId => throw new NotImplementedException();

        string ICandidateAssistant.username => throw new NotImplementedException();

        string ICandidateAssistant.password => throw new NotImplementedException();
    }
}
