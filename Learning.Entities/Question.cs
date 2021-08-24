using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.Entities
{
   public class Question
    {
        [Key]
        public int QusID { get; set; }
        public string QuestionName { get; set; }
        public int QusType { get; set; }
        public int TestId { get; set; }
        public int SectionId { get; set; }
        public int Mark { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }


    }
}
