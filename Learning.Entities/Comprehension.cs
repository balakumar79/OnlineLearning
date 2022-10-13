using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities
{
   public class Comprehension
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
        public int QusId { get; set; }
        public virtual Question  Question { get; set; }
        public int CompQusId { get; set; }
        public int SectionId { get; set; }
        public virtual TestSection TestSection { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
