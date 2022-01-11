using Learning.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.ViewModel.Account
{
   public class AuthorizationModel
    {
        public static class Permissions
        {
            public static class Roles
            {
                public const string Admin = "Admin";
                public const string Tutor = "Tutor";
                public const string Teacher = "Teacher";
                public const string Parent = "Parent";
                public const string Student = "Student";
            }
            public static class Tutor
            {
                public const string DashBoardView = "Permissions.Tutor.Dashboard.View";
                public const string ProfileEdit = "Permissions.Tutor.Profile.Edit";
                public const string ViewExams = "Permissions.Tutor.Exams.View";
            }

            public static class Student
            {
                public const string DashboardView = "Permissions.Student.Dashboard.View";
                public const string Edit = "Permissions.Student.Profile.Edit";
                public const string Examination = "Permissions.Student.Examination.View";
            }
            public static class Parent
            {
                public const string StudentCreate = "Permissions.Parent.Student.Create";
            }
            public static class Administrator
            {
                public const string Admin = "Admin";
            }
        }

        public class CustomClaimTypes
        {
            public const string Permission = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/permission";
            public const string TutorID = "tutorId";
            public const string Role = "role";
            public const string RoleId = "roleid";
            public const string StudentId = "studentid";
            public const string ChildIds = "childIds";


        }
        public const string IdentityApplicationDefault = "Identity.Application";    
        public class ScreenFormeter
        {
            public string ScreenName { get; set; }
        }
    }
}
