using Learning.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Learning.Tutor.ViewModel
{
    public class TestViewModel
    {
        public TestViewModel()
        {
            LanguageVariants = LanguageVariants ?? new List<SelectListItem>();
        }
        public int Id { get; set; }

        [Required]
        [Remote(controller: "Tutor", action: "IsTestExists", AdditionalFields = "Id,TutorId")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Grade")]
        public int GradeID { get; set; }
        public string GradeName { get; set; }
        [Required]
        [Display(Name = "Subject")]
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }

        public string Topics { get; set; }
        public string SubTopics { get; set; }

        public int? TopicId { get; set; }
        public int? SubTopicId { get; set; }

        [Required]
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        [Required]
        public int Language { get; set; }
        public List<SelectListItem> LanguageVariants { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public DateTime Created { get; set; }
        public int RoleId { get; set; }
        public DateTime Modified { get; set; }
        public int CreatedBy { get; set; }
        public string TutorUserName { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        public int PassingMark { get; set; }

        public decimal? MaximumMarkScored { get; set; }
        public decimal? MinimumMarkScored { get; set; }
        public decimal? AverageScore { get; set; }
        public int TestTypeId { get; set; }
        private int _testType;
        public string TestType { get => ((TestTypeEnum)_testType).ToString(); set => _testType = TestTypeId; }
    }
}
