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
        //string apikey = "eDpl4Sza78-8AkByCkAjfUHCmWzeU8SNWNJui2zaidWM";
        //string url = "https://gateway.watsonplatform.net/assistant/api";
        //string versionDate = "2019-06-25";
        //string workspaceId = "97edd9ef-06e4-4bc3-98c5-65ed9046a5ed";
        //string username = "apikey";
        //string password = "ejxYN6Ojlb2J3TZIauB0yCyaV-Bm8eYyhpQRhq3GTcG1";
        string apikey = "KSgEEAC3nnmAKGYOZwZG_OYc0ddA8Fm74kPPI4VX-65l";
        string url = "https://gateway-wdc.watsonplatform.net/assistant/api";
        string versionDate = "2019-06-25";
        string workspaceId = "a8ea7b60-bcfb-4836-8dfe-f120ac495932";
        string username = "apikey";
        string password = "KSgEEAC3nnmAKGYOZwZG_OYc0ddA8Fm74kPPI4VX-65l";

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
            //var response = service.ListLogs(workspaceId);
            var response = service.ListAllLogs(null);
            CreateConversationModelListFromResponse(response);
        }

        private void CreateAutoMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CandidateModel, Conversation>()
                    .ForMember(c => c.Date, opt => opt.Ignore())
                    .ForMember(c => c.ChatLog, opt => opt.Ignore())
                    .ForMember(c => c.Email, opt => opt.Ignore())
                    .ForMember(c => c.Score, opt => opt.Ignore())
                    .ForMember(c => c.Guid, opt => opt.MapFrom(src => src.ConversationId))
                    .ForMember(c => c.CandidateId, opt => opt.Ignore());
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

            foreach (var model in retVal)
            {
                model.BuildConversationString();
                var conv = _mapper.Map<Conversation>(model);
                conversations.Add(conv);
            }
            _context.Conversations.AddRange(conversations);
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
