using Learning.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Learning.ViewModel.Account
{
    public class RegisterViewModel : AccountUserModel
    {
        public int Role { get; set; }
        //[Required]
        //public string ParentFirstName { get; set; }
        //public string ParentLastName { get; set; }
        //[Required]
        //public string ParentGender { get; set; }

        //[Required]
        //[DataType(DataType.EmailAddress)]
        //[Remote(action: "IsEmailExists", controller: "Account")]

        //public string ParentEmail { get; set; }
        //[Required]
        //public string ParentPassword { get; set; }
        //[Required]
        //public string ParentConfirmPassword { get; set; }
        //[Required]
        //public string ParentPhoneNumber { get; set; }
        //[Required]
        //[Remote(action:"IsUserNameExists",controller:"Account")]
        //public string ParentUserName { get; set; }
        //public bool HasUserAccess { get; set; }
        public StudentModel StudentModel { get; set; }


    }
    public class StudentModel
    {
        public int Id { get; set; }
        [Required]
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        [Required]
        public int StudentGenderId { get; set; }
        private int _studentGenderId;
        public Gender StudentGender { get => (Gender)_studentGenderId; set => _studentGenderId = StudentGenderId; }
        [Required]
        public string StudentPassword { get; set; }
        [Required]
        public string StudentConfirmPassword { get; set; }
        [Required, Remote(controller: "Account", action: "IsStudentUserNameExists")]
        public string StudentUserName { get; set; }
        public int UserId { get; set; }

        public string Institution { get; set; }
        public string StudentDistrict { get; set; }
        public string LanguageKnown { get; set; }
        public int MotherTongue { get; set; }
        public bool IsRegisteredAsMajor { get; set; }
        [JsonPropertyName("StudentAccountRecoveryAnswers")]
        public List<AccountRecoveryAnswerModel> StudentAccountRecoveryAnswerModel { get; set; }

        [Required]
        public int GradeLevels { get; set; }
        private int _roleRequestes;
        public int RoleRequested { get => _roleRequestes; set => _roleRequestes = value; }
        public Roles Roles { get => (Roles)_roleRequestes; }
        public AccountUserModel AccountUserModel { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class TeacherModel
    {
        public int TeacherId { get; set; }
        public int UserId { get; set; }
    }

}