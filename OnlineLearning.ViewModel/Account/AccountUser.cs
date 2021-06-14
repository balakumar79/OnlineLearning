using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Learning.ViewModel.Account
{
    public class AccountUser
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }

        [Required]
        [Remote(action: "IsEmailExists", controller: "Account")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [Remote(action: "IsUserNameExists", controller: "Account")]
        public string UserName { get; set; }
        public bool HasUserAccess { get; set; }

    }
}
