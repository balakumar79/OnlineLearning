using Learning.Entities.Enums;
using Learning.ViewModel.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Learning.Tutor.ViewModel
{
    public class TestViewModel
    {
        public TestViewModel()
        {
            LanguageVariants = LanguageVariants ?? new List<SelectListItem>();
            ShuffleTypeList = ShuffleTypeList ?? new List<SelectListItem>();
        }
        public int Id { get; set; }

        [Required]
        [Remote(controller: "Exam", action: "IsTestExists", AdditionalFields = "Id,TutorId")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Grade")]
        public int GradeID { get; set; }
        public string GradeName { get; set; }
        [Required]
        [Display(Name = "Subject")]
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        [Display(Name = "Shuffle Type")]
        public int ShuffleTypeId { get; set; }
        public string Topics { get; set; }
        public string SubTopics { get; set; }

        [NonZero(false)]
        public int? TopicId { get; set; }
        [NonZero(false)]
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
        [NonZero]
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
        private int _shuffleTypeId;
        public ShuffleTypeEnum SuffleType { get => ((ShuffleTypeEnum)_shuffleTypeId); set => _shuffleTypeId = ShuffleTypeId; }
        public IList<SelectListItem> ShuffleTypeList { get; set; }
    }
}
