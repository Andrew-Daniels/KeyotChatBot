using System;
using Microsoft.EntityFrameworkCore;
using KeyotBotCoreApp.Context.Entities;

namespace KeyotBotCoreApp.Context
{
    public interface IDatabaseContext: IDisposable
    {
        DbSet<Candidate> Candidates { get; set; }
    }
}
