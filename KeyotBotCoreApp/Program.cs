using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using KeyotBotCoreApp.Context;
using KeyotBotCoreApp.Context.Entities;
using KeyotBotCoreApp.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KeyotBotCoreApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread conversationLogThread = new Thread(ProcessConversationLog);
            conversationLogThread.Start();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void ProcessConversationLog()
        {
            while (true)
            {
                try
                {
                    WatsonAssistantService<SeniorCandidateAssistant, SeniorCandidateModel, SeniorCandidate> service = new WatsonAssistantService<SeniorCandidateAssistant, SeniorCandidateModel, SeniorCandidate>();
                    service.StartSession();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread.Sleep(2 * 60 * 1000);
            }
        }
    }
}
