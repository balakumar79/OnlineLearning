using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learning.ViewModel.Extension
{
    public class NonZeroAttribute : ValidationAttribute
    {
        readonly bool isRequired;
        public NonZeroAttribute(bool isRequired = true) : base("The {0} field must not be zero.") => this.isRequired = isRequired;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && !isRequired)
            {
                return ValidationResult.Success;
            }
            if (!int.TryParse(value?.ToString(), out int intValue))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
