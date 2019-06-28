using System;
using System.Collections.Generic;
using IBM.Watson.Assistant.v1.Model;
using KeyotBotCoreApp.Context;
using KeyotBotCoreApp.Context.Entities;

namespace KeyotBotCoreApp.Services
{
    public class BaseCandidateModel
    {
        public string ConversationId { get; set; }
        public List<Log> ConversationLog { get; set; }
        public string ConversationLogString { get; set; }
        public string Name { get; set; }

        public BaseCandidateModel()
        {
            ConversationLog = new List<Log>();
        }

        virtual public void BuildModel()
        { }

        virtual public void Save<T>(CandidateContext context, List<T> candidates)
        { }

        virtual public bool CheckIfCandidateExists(CandidateContext context)
        { return false; }
    }
}
