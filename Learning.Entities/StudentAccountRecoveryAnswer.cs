using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class StudentAccountRecoveryAnswer
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
