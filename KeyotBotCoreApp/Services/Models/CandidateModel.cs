using System;
using System.Collections.Generic;
using IBM.Watson.Assistant.v1.Model;
using KeyotBotCoreApp.Context;

namespace KeyotBotCoreApp.Services
{
    public class CandidateModel: BaseCandidateModel
    {
        public CandidateModel(): base()
        {
        }

        override public void BuildModel()
        {

        }

        override public void Save<T>(CandidateContext context, List<T> candidates)
        { 
        }

        override public bool CheckIfCandidateExists(CandidateContext context)
        {
            return false;
        }
    }
}
