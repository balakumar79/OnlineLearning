using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.Tutor.ViewModel
{
   public class TestViewModel
    {
        public int Id { get; set; }

        [Required]
        [Remote(controller:"Tutor",action:"IsTestExists",AdditionalFields ="Id")]
        public string Title { get; set; }
        [Required]
        [Display(Name ="Grade")]
        public int GradeID { get; set; }
        public string GradeName { get; set; }
        [Required]
        [Display(Name ="Subject")]
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }

        public string Topics { get; set; }
        public string SubTopics { get; set; }
        [Required]
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public int Language { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }

        public DateTime Created { get; set; }
        
        public DateTime Modified { get; set; }
        public string TutorId { get; set; }
    }
}
