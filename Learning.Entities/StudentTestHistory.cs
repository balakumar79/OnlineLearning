using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class StudentTestHistory
    {
        public int Id { get; set; }
        public int StudentTestId { get; set; }
        public int StudentId { get; set; }
        public DateTime AttemptedAt { get; set; }
        public string TimeTaken { get; set; }
        public int Score { get; set; }
        public string CorrectAnswers { get; set; }
        public int TotalMarks { get; set; }

    }
}
