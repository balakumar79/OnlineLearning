using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.ViewModel.Account
{
    public class ResetPasswordViewModel
    {
        [Required]
        [Display(Name = "New Password")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }

    }
    public class ResetStudentPasswordModel 
    {
        [Required]
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        [Required(ErrorMessage ="Secret answer cannot be blank")]
        public string Answer { get; set; }
        [Required]
        [Display(Name = "New Password")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}