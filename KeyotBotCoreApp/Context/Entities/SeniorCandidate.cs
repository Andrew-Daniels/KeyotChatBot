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
        public string CandidateId { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("futureMeetTime")]
        public string FutureMeetTime { get; set; }
        [Column("interested")]
        public string Interested { get; set; }
        [Column("phone")]
        public string Phone { get; set; }
        [Column("preferredMethodContact")]
        public string PreferredContactMethod { get; set; }
        [Column("roles")]
        public string Roles { get; set; }
    }
}
