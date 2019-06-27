using System;
using System.Collections.Generic;
using System.Linq;
using IBM.Watson.Assistant.v1.Model;

namespace KeyotBotCoreApp.Services
{
    public class CandidateModel
    {
        public int CandidateId { get; set; }
        public string ConversationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Log> ConversationLog { get; set; }
        public string ConversationLogString { get; set; }

        public CandidateModel()
        {
            ConversationLog = new List<Log>();
        }

        public void BuildConversationString()
        {
            ConversationLog = ConversationLog.OrderBy(c => c.ResponseTimestamp).ToList();

            for (int i = 0; i < ConversationLog.Count; i ++)
            {
                var input = ConversationLog[i].Request.Input.Text;
                var output = ConversationLog[i].Response.Output.Text.FirstOrDefault();

                switch (i)
                {
                    case 0:
                        FirstName = input;
                        break;
                    case 1:
                        LastName = input;
                        break;
                    case 2:
                        Email = input;
                        break;
                    default:
                        ConversationLogString += String.Format("Bot says:\n\r\t{0}\n\r{1} says:\n\r\t{2}\n\r", output, FirstName, input);
                        break;
                }
            }
        }
    }
}
