using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class TrueOrFalse
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Options { get; set; }
        public bool IsCorrect { get; set; }
        public int Position { get; set; }
    }
}
