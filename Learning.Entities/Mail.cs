using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class Mail
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ToAddress { get; set; }
        public DateTime SentOn { get; set; }
        public DateTime ReadOn { get; set; }
        public string SenderAddress { get; set; }
        public string Attachement { get; set; }
    }
}
