using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.Tutor.ViewModel
{
   public class TestSectionViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Test")]
        public int TestId { get; set; }
        public string TestName { get; set; }
        [Required]
        [Remote(controller:"Tutor",action:"IsSectionExists",AdditionalFields ="Id")]
        public string SectionName { get; set; }
        [Required]
        public string Topic { get; set; }
        public string SubTopic { get; set; }
        [Required]
        public int TotalMarks { get; set; }
        [Required]
        public int TotalQuestions { get; set; }
        [Display(Name ="Additional Instruction")]
        public string AdditionalInstruction { get; set; }
        public int AddedQuestions { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        [Required]
        [Display(Name ="Online")]
        public bool IsOnline { get; set; }
    }
}
