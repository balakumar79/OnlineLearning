using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class TutorGradesTaken
    {
        public int Id { get; set; }
        public int TutorID { get; set; }
        public int GradeID { get; set; }
    }
}
