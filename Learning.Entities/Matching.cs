using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class Matching
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Sentence { get; set; }
        public string CorrectAnswer { get; set; }
        public int Position { get; set; }
    }
}
