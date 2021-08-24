using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.Entities
{
   public class Test
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string TestDescription { get; set; }
        public int GradeID { get; set; }
        public int SubjectID { get; set; }
        public string Topics { get; set; }
        public string SubTopics { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PassingMark { get; set; }
        public int StatusID { get; set; }
        public int Language { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string TutorId { get; set; }

    }
}
