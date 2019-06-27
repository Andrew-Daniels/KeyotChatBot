using System;
using KeyotBotCoreApp.Context.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyotBotCoreApp.Context
{
    public class CandidateContext: DbContext
    {
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<SeniorCandidate> SeniorCandidates { get; set; }
        public DbSet<Conversation> Conversations { get; set; }

        public CandidateContext(DbContextOptions<CandidateContext> options): base(options)
        {
        }
    }
}
