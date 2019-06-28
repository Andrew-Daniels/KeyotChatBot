using System;
using System.Collections.Generic;
using System.Linq;
using IBM.Watson.Assistant.v1.Model;
using KeyotBotCoreApp.Context;
using KeyotBotCoreApp.Context.Entities;

namespace KeyotBotCoreApp.Services
{
    public class SeniorCandidateModel: BaseCandidateModel
    {
        public string Email { get; set; }
        public string FutureMeetTime { get; set; }
        public string Interested { get; set; }
        public string Phone { get; set; }
        public string PreferredContactMethod { get; set; }
        public string Roles { get; set; }

        public SeniorCandidateModel(): base()
        {
        }

        override public void BuildModel()
        {
            try 
            {
                ConversationLog = ConversationLog.OrderBy(c => c.ResponseTimestamp).ToList();

                if (ConversationLog.Count > 2)
                {
                    Name = ConversationLog[3].Request.Input.Text;
                }
                else 
                {
                    return;
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

        override public void Save<T>(CandidateContext context, List<T> candidates)
        {
            context.SeniorCandidates.AddRange(candidates as List<SeniorCandidate>);
        }

        override public bool CheckIfCandidateExists(CandidateContext context)
        {
            return context.SeniorCandidates.Where(s => s.CandidateId == ConversationId).FirstOrDefault() != null;
        }
    }
}
