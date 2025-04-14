using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.Model
{
    public class Response
    {
        [Key]
        public int ResponseId { get; set; }

        public int PlayerId { get; set; }

        public int QuestionId { get; set; }

        public int AnswerId { get; set; }

        public int ResponseTime { get; set; }

        public int Points { get; set; }

        public int Streak { get; set; }

        // Quan hệ
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        [ForeignKey("AnswerId")]
        public Answer Answer { get; set; }
    }
}
