using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class Logger
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
