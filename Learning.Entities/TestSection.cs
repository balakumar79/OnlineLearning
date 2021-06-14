using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.Entities
{
   public class TestSection
    {
        [Key]
        public int Id { get; set; }
        public int TestId { get; set; }
        public string SectionName { get; set; }
        public string SubTopic { get; set; }
        public int TotalMarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool? IsOnline { get; set; }

    }
}
