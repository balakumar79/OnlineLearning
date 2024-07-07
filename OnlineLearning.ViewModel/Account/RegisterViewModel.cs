using Learning.Entities.Enums;
using Learning.Utils.Common;
using Learning.Utils.Common.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Learning.ViewModel.Account
{
    public class RegisterViewModel : AccountUserModel
    {
        public RegisterViewModel()
        {

        }

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
        public StudentModel()
        {

        }

        public StudentModel(int id, string studentFirstName, string studentLastName, int studentGenderId, Gender studentGender, string studentPassword, string studentConfirmPassword, string studentUserName, int userId, string institution, string studentDistrict, string languageKnown, int motherTongue, bool isRegisteredAsMajor, List<AccountRecoveryAnswerModel> studentAccountRecoveryAnswerModel, int gradeLevels, int roleRequestes, int roleRequested, AccountUserModel accountUserModel, DateTime? createdOn)
        {
            Id = id;
            StudentFirstName = studentFirstName;
            StudentLastName = studentLastName;
            _studentGenderId = studentGenderId;
            _studentGenderId = studentGenderId;
            StudentGender = studentGender;
            StudentPassword = studentPassword;
            StudentConfirmPassword = studentConfirmPassword;
            StudentUserName = studentUserName;
            UserId = userId;
            Institution = institution;
            StudentDistrict = studentDistrict;
            LanguageKnown = languageKnown;
            MotherTongue = motherTongue;
            IsRegisteredAsMajor = isRegisteredAsMajor;
            StudentAccountRecoveryAnswerModel = studentAccountRecoveryAnswerModel;
            GradeLevels = gradeLevels;
            _roleRequestes = roleRequestes;
            RoleRequested = roleRequested;
            AccountUserModel = accountUserModel;
            CreatedOn = createdOn;
        }

        public int Id { get; set; }
        [Required,Display(Name =nameof(StudentFirstName))]
        [MaxLength(50)]
        [RegularExpression(CommonRegularExpressionClr.AlphaNumericOnly, ErrorMessageResourceName = LocalizerConstant.ONLY_ALPHAPEHET_ALLOWED,
            ErrorMessageResourceType = typeof(Resource))]
        public string StudentFirstName { get; set; }
        [RegularExpression(CommonRegularExpressionClr.AlphaNumericOnly,ErrorMessageResourceName =LocalizerConstant.ONLY_ALPHAPEHET_ALLOWED,
            ErrorMessageResourceType =typeof(Resource))]
        public string StudentLastName { get; set; }
        [Required]
        public int StudentGenderId { get; set; }
        private int _studentGenderId;
        public Gender StudentGender { get => (Gender)_studentGenderId; set => _studentGenderId = StudentGenderId; }
        [Required]
        [MaxLength(50),MinLength(5)]
        public string StudentPassword { get; set; }
        [Required]
        [MaxLength(50),MinLength(5)]
        public string StudentConfirmPassword { get; set; }
        [MaxLength(50)]
        [RegularExpression(CommonRegularExpressionClr.AlphaNumericWithSpecial, ErrorMessageResourceName = LocalizerConstant.ALPHANUMERICWITHSPECIAL,
           ErrorMessageResourceType = typeof(Resource))]
        [Required, Remote(controller: "Account", action: "IsStudentUserNameExists")]
        public string StudentUserName { get; set; }
        public int UserId { get; set; }
        [MaxLength(150)]
        [RegularExpression(CommonRegularExpressionClr.AlphaNumericWithSpecial, ErrorMessageResourceName = LocalizerConstant.ALPHANUMERICWITHSPECIAL,
        ErrorMessageResourceType = typeof(Resource))]
        public string Institution { get; set; }
        [MaxLength(50)]
        [RegularExpression(CommonRegularExpressionClr.AlphaNumericWithSpecial, ErrorMessageResourceName = LocalizerConstant.ALPHANUMERICWITHSPECIAL,
           ErrorMessageResourceType = typeof(Resource))]
        public string StudentDistrict { get; set; }
        [MaxLength(50)]
        [RegularExpression(CommonRegularExpressionClr.AlphaNumericWithSpecial, ErrorMessageResourceName = LocalizerConstant.ALPHANUMERICWITHSPECIAL,
           ErrorMessageResourceType = typeof(Resource))]
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
        public AccountUserModel UserModel { get; set; }
    }

}