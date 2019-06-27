using System;
using KeyotBotCoreApp.Context.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyotBotCoreApp.Context
{
    public interface IConversationContext: IDisposable
    {
        DbSet<Candidate> Candidates { get; set; }
    }
}
