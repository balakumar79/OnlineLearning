using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class StudentTestHistory
    {
        public int Id { get; set; }
        public int StudentTestId { get; set; }
        public DateTime AttemptedAt { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public int CorrectAnswers { get; set; }
        public int Marks { get; set; }
    }
}
