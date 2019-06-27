using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeyotBotCoreApp.Context.Entities
{
    [Table("Candidate_SR")]
    public class SeniorCandidate
    {
        [Key]
        [Column("candidate_id")]
        public int CandidateId { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("firstName")]
        public string FirstName { get; set; }
        [Column("lastName")]
        public string LastName { get; set; }
        [Column("futureMeetTime")]
        public string FutureMeetTime { get; set; }
        [Column("interested")]
        public string Interested { get; set; }
        [Column("phone")]
        public int Phone { get; set; }
        [Column("prefferredMethodContact")]
        public string PreferredContactMethod { get; set; }
        [Column("roles")]
        public int Roles { get; set; }
    }
}
