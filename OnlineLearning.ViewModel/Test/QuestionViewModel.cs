using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.Tutor.ViewModel
{
   public class QuestionViewModel
    {
        public int QusID { get; set; }
        [Required]

        [Display(Name ="Qus.Name")]
        public string QuestionName { get; set; }
        
        public string QusType { get; set; }
        [Required]
        [Display(Name ="Qus.Type")]
        public int QuestionTypeId { get; set; }
        [Required]
        public int TestId { get; set; }
        public string TestName { get; set; }
        public int Language { get; set; }
        public int SectionId { get; set; }
        [Display(Name ="Sec.Name")]
        public string SectionName { get; set; }
        public List<OptionsViewModel> Options { get; set; }
        public int Mark { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
    public class OptionsViewModel
    {
        public int Id { get; set; }
   [Required]
        public string Option { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public int Position { get; set; }
    }
}
