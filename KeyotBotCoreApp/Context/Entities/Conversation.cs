using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeyotBotCoreApp.Context.Entities
{
    [Table("Conversation")]
    public class Conversation
    {
        [Column("candidate_email")]
        public string Email { get; set; }
        [Column("candidate_id")]
        public int CandidateId { get; set; }
        [Column("chate_date")]
        public DateTime Date { get; set; }
        [Column("chat_log")]
        public string ChatLog { get; set; }
        [Key]
        [Column("guid")]
        public string Guid { get; set; }
        [Column("score")]
        public int Score { get; set; }
    }
}
