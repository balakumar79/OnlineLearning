using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Entities.Config
{
    class AppConstants
    {
    }
    public static class EmailTemplate
    {
        public readonly static string ConfirmEmail = "ConfirmEmail.html";
        public readonly static string ResetPassword = "ForgotPassword.html";
        public readonly static string SendStudentInvitationLinkByTeacher = "SendStudentInvitationLinkByTeacher.html";
    }
    public static class RolesConstant
    {
        public const string Admin = "Admin";
        public const string Teacher = "Teacher";
    }
}
