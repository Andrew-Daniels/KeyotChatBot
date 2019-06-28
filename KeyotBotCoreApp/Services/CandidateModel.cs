using System;
using System.Collections.Generic;
using System.Linq;
using IBM.Watson.Assistant.v1.Model;

namespace KeyotBotCoreApp.Services
{
    public class CandidateModel
    {
        public string ConversationId { get; set; }
        public List<Log> ConversationLog { get; set; }
        public string ConversationLogString { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string FutureMeetTime { get; set; }
        public string Interested { get; set; }
        public string Phone { get; set; }
        public string PreferredContactMethod { get; set; }
        public string Roles { get; set; }

        public CandidateModel()
        {
            ConversationLog = new List<Log>();
        }

        public void BuildModel()
        {
            try 
            {
                ConversationLog = ConversationLog.OrderBy(c => c.ResponseTimestamp).ToList();

                if (ConversationLog.Count > 2)
                {
                    Name = ConversationLog[3].Request.Input.Text;
                }

                for (int i = 0; i < ConversationLog.Count; i++)
                {
                    var input = "";
                    if (ConversationLog.Count > i)
                    {
                        input = ConversationLog[i + 1].Request.Input.Text;
                    }
                    var outputs = ConversationLog[i].Response.Output.Text;

                    switch (i)
                    {
                        case 0:
                            Interested = input;
                            break;
                        case 1:
                            Roles = input;
                            break;
                        case 3:
                            Phone = input;
                            break;
                        case 4:
                            Email = input;
                            break;
                        case 5:
                            FutureMeetTime = input;
                            break;
                        case 6:
                            PreferredContactMethod = input;
                            break;
                    }
                    string botSays = "";
                    foreach (var output in outputs)
                    {
                        botSays += String.Format(@"<p>Bot says:</p><p>{0}<p>", output);
                    }
                    ConversationLogString += String.Format("{0}<p>{1} says:</p><p>{2}</p>", botSays, Name, input);
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
