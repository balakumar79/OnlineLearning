using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class MCQAnswer
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
       

        public int QuestionId { get; set; }
        public int Position { get; set; }
        public virtual Question Question { get; set; }
    }
}
