using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RequestAndResponse.Request.QuestionInGameRequest
{
    public class UpdateQuestionInGameRequest
    {
        public int QuestionInGameId { get; set; }
        public int SessionId { get; set; }
        public int QuestionId { get; set; }
        public int OrderIndex { get; set; }
        public DateTime CreatedTime { get; set; }
        public int TotalMembers { get; set; }
        //public string Status {  get; set; }
    }
}
