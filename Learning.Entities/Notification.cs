using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public int Type { get; set; }
        public int Action { get; set; }
        public bool IsActive { get; set; }
        public bool Read { get; set; }
        public DateTime ReadOn { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
