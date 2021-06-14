using Learning.Entities;
using Learning.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.Tutor.ViewModel
{
    public class TutorViewModel:AccountUser
    {
        public int TutorID { get; set; }
        public string TutorType { get; set; }
        public string Organization { get; set; }
        public string LanguagePreference { get; set; }
        public List<int> LanguageOfInstruction { get; set; }
        public string Educations { get; set; }
        public List<int> GradesTaken { get; set; }
        public DateTime LastLoggedIn { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
