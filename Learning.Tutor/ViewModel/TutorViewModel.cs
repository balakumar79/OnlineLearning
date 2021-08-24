using Learning.Entities;
using Learning.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.Tutor.ViewModel
{
    public class TutorViewModel:AccountUserModel
    {
        public int TutorID { get; set; }
        public string TutorType { get; set; }
        public string Organization { get; set; }
        [Required]
        public string LanguagePreference { get; set; }
        [Required]
        public List<int> LanguageOfInstruction { get; set; }
        public string Educations { get; set; }
        public List<int> GradesTaken { get; set; }
        public DateTime LastLoggedIn { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
