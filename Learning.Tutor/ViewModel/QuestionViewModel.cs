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
        public string QuestionName { get; set; }
        
        public string QusType { get; set; }
        [Required]
        public int QuestionTypeId { get; set; }
        [Required]
        public int TestId { get; set; }
        public string TestName { get; set; }
        [Required]
        public int SectionId { get; set; }
        public int SectionName { get; set; }
        public List<OptionsViewModel> Options { get; set; }
        public int IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
    public class OptionsViewModel
    {
        public int Id { get; set; }
   [Required]
        public string Option { get; set; }
        public bool IsCorrect { get; set; }
    }
}
