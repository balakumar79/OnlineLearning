using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class QuestionAdditionalInfo
    {
        public int Id { get; set; }
        public int QusId { get; set; }
        public string Topic { get; set; }
        public string SubTopic { get; set; }
        public bool Active { get; set; }
    }
}
