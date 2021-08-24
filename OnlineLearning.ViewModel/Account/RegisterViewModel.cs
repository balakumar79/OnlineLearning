using Learning.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.ViewModel.Account
{
    public class RegisterViewModel:AccountUserModel
    {

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

        [Required]
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        [Required]
        public string StudentGender { get; set; }
        [Required]
        public string StudentPassword { get; set; }
        [Required]
        public string StudentConfirmPassword { get; set; }
        [Required]
        public string StudentUserName { get; set; }

       
        [Required]
        public int GradeLevels { get; set; }
       
    }
}