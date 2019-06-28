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
    public class WatsonAssistantService<T, R, V> where T: ICandidateAssistant, new() where R: BaseCandidateModel, new()
    {
        private readonly CandidateContext _context;
        private IMapper _mapper;
        private T _candidateAssistant;
        private List<R> _model;

        public WatsonAssistantService()
        {
            _model = new List<R>();
            _candidateAssistant = new T();
            var optionsBuilder = new DbContextOptionsBuilder<CandidateContext>();
            optionsBuilder.UseMySQL("Server=remotemysql.com;Database=sAzFragJ80;Uid=sAzFragJ80;Pwd=Ye5x0LBDEk;");
            _context = new CandidateContext(optionsBuilder.Options);
            CreateAutoMapper();
        }

        #region Sessions
        public void StartSession()
        {
            var service = new AssistantService(_candidateAssistant.username, _candidateAssistant.password, _candidateAssistant.versionDate);
            service.SetEndpoint(_candidateAssistant.url);
            service.ApiKey = _candidateAssistant.apikey;
            var response = service.ListLogs(_candidateAssistant.workspaceId, "-request_timestamp");
            CreateConversationModelListFromResponse(response);
        }

        private void CreateAutoMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SeniorCandidateModel, Conversation>()
                    .ForMember(c => c.Date, opt => opt.MapFrom(src => DateTime.Parse(src.ConversationLog.FirstOrDefault().RequestTimestamp)))
                    .ForMember(c => c.ChatLog, opt => opt.MapFrom(src => src.ConversationLogString ?? ""))
                    .ForMember(c => c.Guid, opt => opt.MapFrom(src => src.ConversationId))
                    .ForMember(c => c.Score, opt => opt.Ignore());
                cfg.CreateMap<SeniorCandidateModel, SeniorCandidate>()
                    .ForMember(c => c.CandidateId, opt => opt.MapFrom(src => src.ConversationId));
            });
            // only during development, validate your mappings; remove it before release
            configuration.AssertConfigurationIsValid();
            _mapper = configuration.CreateMapper();
        }

        private void CreateConversationModelListFromResponse(IBM.Cloud.SDK.Core.Http.DetailedResponse<LogCollection> response)
        {
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

                    ExtractCandidateAndConversationFromResponse(log, conversationId);
                }

                if (_model.Count > 0)
                {
                    SaveCandidatesAndConversations();
                }
            }
        }

        private void ExtractCandidateAndConversationFromResponse(Log log, string conversationId)
        {
            if (_model.Count > 0)
            {
                var match = _model.FirstOrDefault(m => m.ConversationId.Equals(conversationId));
                if (match != null)
                {
                    match.ConversationLog.Add(log);
                    match.ConversationLog = match.ConversationLog.OrderBy(m => DateTime.Parse(m.ResponseTimestamp)).ToList();
                }
                else
                {
                    AddLogToConversationModelList(log, conversationId);
                }
            }
            else
            {
                AddLogToConversationModelList(log, conversationId);
            }
        }

        private void SaveCandidatesAndConversations()
        {
            List<Conversation> conversations = new List<Conversation>();
            List<V> candidates = new List<V>();

            foreach (var model in _model)
            {
                model.BuildModel();
                var conv = _mapper.Map<Conversation>(model);
                var candidate = _mapper.Map<V>(model);

                if (!String.IsNullOrWhiteSpace(conv.ChatLog))
                    conversations.Add(conv);

                if (model.CheckIfCandidateExists(_context) && !String.IsNullOrWhiteSpace(model.Name))
                    candidates.Add(candidate);
                    
            }
            _context.Conversations.AddRange(conversations);
            _model.FirstOrDefault().Save<V>(_context, candidates);
            _context.SaveChanges();
        }

        private void AddLogToConversationModelList(Log log, string conversationId)
        {
            var conv = new R()
            {
                ConversationId = conversationId,
                ConversationLog = new List<Log>() { log }
            };
            _model.Add(conv);
        }
        #endregion
    }
}
