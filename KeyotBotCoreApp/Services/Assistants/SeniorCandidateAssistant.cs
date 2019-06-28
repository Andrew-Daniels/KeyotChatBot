using System;
using System.Collections.Generic;

namespace KeyotBotCoreApp.Services
{
    public class SeniorCandidateAssistant : ICandidateAssistant
    {
        public SeniorCandidateAssistant()
        {
        }

        public string apikey => "XcnidYn9tQOMdadWG36fBHJBF6S9OFQiXLWJtjob1xN-";

        public string url => "https://gateway.watsonplatform.net/assistant/api";

        public string versionDate => "2019-06-27";

        public string workspaceId => "fcaefbc7-bbe1-418d-9a65-18a5314ac553";

        public string username => "apikey";

        public string password => "XcnidYn9tQOMdadWG36fBHJBF6S9OFQiXLWJtjob1xN-";
    }
}
