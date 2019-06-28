using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeyotBotCoreApp.Context.Entities
{
    [Table("Conversation")]
    public class Conversation
    {
        [Key]
        [Column("guid")]
        public string Guid { get; set; }
        [Column("chat_date")]
        public DateTime Date { get; set; }
        [Column("chat_log")]
        public string ChatLog { get; set; }
        [Column("score")]
        public int Score { get; set; }
    }
}
