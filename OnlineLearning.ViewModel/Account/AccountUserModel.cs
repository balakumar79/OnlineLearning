using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Learning.ViewModel.Account
{
    public class AccountUserModel
    {
        public int UserID { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public ViewModel.Enums.Genders Gender { get; set; }

        [Required,DataType(DataType.EmailAddress)]
        [Remote(action: "IsEmailExists", controller: "Account")]
        public string Email { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        [Required,DataType(DataType.Password),Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required,DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        [Remote(action: "IsUserNameExists", controller: "Account")]
        public string UserName { get; set; }
        public int MotherTongue { get; set; }
        public string HearAbout { get; set; }
        public string District { get; set; }
        public bool HasUserAccess { get; set; }

    }
}
