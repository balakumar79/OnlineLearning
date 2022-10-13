using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
    public class StudentInvitation
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int Parentid { get; set; }
        public int TeacherId { get; set; }
        public int Response { get; set; }
        public DateTime SentOn { get; set; }
        public DateTime? AcceptedOn { get; set; }
    }
}
