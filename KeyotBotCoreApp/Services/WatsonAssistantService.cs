using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using IBM.Cloud.SDK.Core.Util;
using IBM.Watson.Assistant.v1;
using IBM.Watson.Assistant.v1.Model;
using KeyotBotCoreApp.Context;
using KeyotBotCoreApp.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KeyotBotCoreApp.Services
{
    public class WatsonAssistantService
    {
        string apikey = "XcnidYn9tQOMdadWG36fBHJBF6S9OFQiXLWJtjob1xN-";
        string url = "https://gateway.watsonplatform.net/assistant/api";
        string versionDate = "2019-06-27";
        string workspaceId = "fcaefbc7-bbe1-418d-9a65-18a5314ac553";
        string username = "apikey";
        string password = "XcnidYn9tQOMdadWG36fBHJBF6S9OFQiXLWJtjob1xN-";

        private readonly CandidateContext _context;
        private IMapper _mapper;

        public WatsonAssistantService()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CandidateContext>();
            optionsBuilder.UseMySQL("Server=remotemysql.com;Database=sAzFragJ80;Uid=sAzFragJ80;Pwd=Ye5x0LBDEk;");
            _context = new CandidateContext(optionsBuilder.Options);
            CreateAutoMapper();
        }

        #region Sessions
        public void StartSession()
        {
            var service = new AssistantService(username, password, versionDate);
            service.SetEndpoint(url);
            service.ApiKey = apikey;
            var response = service.ListLogs(workspaceId, "-request_timestamp");
            //var response = service.ListAllLogs("language::en,request.context.metadata.deployment::Development");
            CreateConversationModelListFromResponse(response);
        }

        private void CreateAutoMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CandidateModel, Conversation>()
                    .ForMember(c => c.Date, opt => opt.MapFrom(src => DateTime.Parse(src.ConversationLog.FirstOrDefault().RequestTimestamp)))
                    .ForMember(c => c.ChatLog, opt => opt.MapFrom(src => src.ConversationLogString ?? ""))
                    .ForMember(c => c.Guid, opt => opt.MapFrom(src => src.ConversationId))
                    .ForMember(c => c.Score, opt => opt.Ignore());
                cfg.CreateMap<CandidateModel, SeniorCandidate>()
                    .ForMember(c => c.CandidateId, opt => opt.MapFrom(src => src.ConversationId));
            });
            // only during development, validate your mappings; remove it before release
            configuration.AssertConfigurationIsValid();
            _mapper = configuration.CreateMapper();
        }

        private void CreateConversationModelListFromResponse(IBM.Cloud.SDK.Core.Http.DetailedResponse<LogCollection> response)
        {
            List<CandidateModel> retVal = new List<CandidateModel>();

            using (_context)
            {
                foreach (var log in response.Result.Logs)
                {
                    var conversationId = log.Response.Context.ConversationId;

                    var conversationExists = _context.Conversations
                        .Where(c => c.Guid == conversationId)
                        .FirstOrDefault() != null;

                    if (conversationExists)
                    {
                        continue;
                    }

                    ExtractCandidateAndConversationFromResponse(retVal, log, conversationId);
                }

                if (retVal.Count > 0)
                {
                    SaveConversations(retVal);
                }
            }
        }

        private void ExtractCandidateAndConversationFromResponse(List<CandidateModel> retVal, Log log, string conversationId)
        {
            if (retVal.Count > 0)
            {
                var match = retVal.FirstOrDefault(m => m.ConversationId.Equals(conversationId));
                if (match != null)
                {
                    match.ConversationLog.Add(log);
                    match.ConversationLog = match.ConversationLog.OrderBy(m => DateTime.Parse(m.ResponseTimestamp)).ToList();
                }
                else
                {
                    AddLogToConversationModelList(retVal, log, conversationId);
                }
            }
            else
            {
                AddLogToConversationModelList(retVal, log, conversationId);
            }
        }

        private void SaveConversations(List<CandidateModel> retVal)
        {
            List<Conversation> conversations = new List<Conversation>();
            List<SeniorCandidate> seniors = new List<SeniorCandidate>();

            foreach (var model in retVal)
            {
                model.BuildModel();
                var conv = _mapper.Map<Conversation>(model);
                var senior = _mapper.Map<SeniorCandidate>(model);

                var candidateExists = _context.SeniorCandidates.Where(s => s.CandidateId == senior.CandidateId).FirstOrDefault() != null;

                if (!String.IsNullOrWhiteSpace(conv.ChatLog))
                    conversations.Add(conv);

                if (!candidateExists && !String.IsNullOrWhiteSpace(senior.Name))
                    seniors.Add(senior);
            }
            _context.Conversations.AddRange(conversations);
            _context.SeniorCandidates.AddRange(seniors);
            _context.SaveChanges();
        }

        private void AddLogToConversationModelList(List<CandidateModel> model, Log log, string conversationId)
        {
            var conv = new CandidateModel()
            {
                ConversationId = conversationId,
                ConversationLog = new List<Log>() { log }
            };
            model.Add(conv);
        }
        #endregion
    }
}
