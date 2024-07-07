using Learning.Utils.Common;
using Learning.Utils.Common.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Learning.ViewModel.Account
{
    public class AccountUserModel
    {
        public int UserID { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression(CommonRegularExpressionClr.AlphaNumericOnly, ErrorMessageResourceName = LocalizerConstant.ONLY_ALPHAPEHET_ALLOWED,
            ErrorMessageResourceType = typeof(Resource))]
        public string FirstName { get; set; }
        [MaxLength(50)]
        [RegularExpression(CommonRegularExpressionClr.AlphaNumericOnly, ErrorMessageResourceName = LocalizerConstant.ONLY_ALPHAPEHET_ALLOWED,
            ErrorMessageResourceType = typeof(Resource))]
        public string LastName { get; set; }
        [MaxLength(50)]
        [RegularExpression(CommonRegularExpressionClr.AlphaNumericOnly, ErrorMessageResourceName = LocalizerConstant.ONLY_ALPHAPEHET_ALLOWED,
            ErrorMessageResourceType = typeof(Resource))]
        public string Gender { get; set; }
        [Required]
        public int GenderId { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        [Remote(action: "IsEmailExists", controller: "Account")]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]

        public bool EmailConfirmed { get; set; }
        [MaxLength(50)]
        [RegularExpression(CommonRegularExpressionClr.PasswordStrength, ErrorMessageResourceName = LocalizerConstant.PASSWORDVALIDATOR,
            ErrorMessageResourceType = typeof(Resource))]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare("Password")]
        [MaxLength(50)]
        [RegularExpression(CommonRegularExpressionClr.PasswordStrength, ErrorMessageResourceName = LocalizerConstant.PASSWORDVALIDATOR,
            ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.PhoneNumber)]
        [MaxLength(50)]
        [RegularExpression(CommonRegularExpressionClr.NumberOnly, ErrorMessageResourceName = LocalizerConstant.NUMBERONLY,
            ErrorMessageResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }
        [Required]
        [Remote(action: "IsUserNameExists", controller: "Account")]
        [MaxLength(50)]
        [RegularExpression(CommonRegularExpressionClr.AlphaNumericWithSpecial, ErrorMessageResourceName = LocalizerConstant.ALPHANUMERICWITHSPECIAL,
            ErrorMessageResourceType = typeof(Resource))]
        public string UserName { get; set; }
        public int MotherTongue { get; set; }
        [MaxLength(500)]
        [RegularExpression(CommonRegularExpressionClr.AlphaNumericOnly, ErrorMessageResourceName = LocalizerConstant.ONLY_ALPHAPEHET_ALLOWED,
            ErrorMessageResourceType = typeof(Resource))]
        public string HearAbout { get; set; }
        [MaxLength(50)]
        [RegularExpression(CommonRegularExpressionClr.AlphaNumericOnly, ErrorMessageResourceName = LocalizerConstant.ONLY_ALPHAPEHET_ALLOWED,
            ErrorMessageResourceType = typeof(Resource))]
        public string District { get; set; }
        public bool HasUserAccess { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
