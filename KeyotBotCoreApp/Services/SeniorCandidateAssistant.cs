using System;
namespace KeyotBotCoreApp.Services
{
    public class SeniorCandidateAssistant: ICandidateAssistant
    {
        public SeniorCandidateAssistant()
        {
        }

        string ICandidateAssistant.apikey => "XcnidYn9tQOMdadWG36fBHJBF6S9OFQiXLWJtjob1xN-";

        string ICandidateAssistant.url => "https://gateway.watsonplatform.net/assistant/api";

        string ICandidateAssistant.versionDate => "2019-06-27";

        string ICandidateAssistant.workspaceId => "fcaefbc7-bbe1-418d-9a65-18a5314ac553";

        string ICandidateAssistant.username => "apikey";

        string ICandidateAssistant.password => "XcnidYn9tQOMdadWG36fBHJBF6S9OFQiXLWJtjob1xN-";
    }
}
