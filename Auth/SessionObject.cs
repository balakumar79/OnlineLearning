using Learning.Entities;
using Learning.Tutor.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using static Learning.ViewModel.Account.AuthorizationModel;

namespace Learning.Auth
{
    public class SessionObject
    {
       
        public AppUser User { get; set; }
        public Tutor.ViewModel.TutorViewModel Tutor { get; set; }
        public List<ScreenFormeter>  ScreenAccess { get; set; }
        public Student Student { get; set; }
        public List<Student> Childs { get; set; }
        public List<string> RoleID { get; set; }
    }
}
