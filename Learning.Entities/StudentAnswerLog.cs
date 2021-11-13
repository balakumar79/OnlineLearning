using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.Entities
{
   public class StudentAnswerLog
    {
        [Key]
        public int LogId { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int TestId { get; set; }
        public Test  Test { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int OptionId { get; set; }
    }
}
