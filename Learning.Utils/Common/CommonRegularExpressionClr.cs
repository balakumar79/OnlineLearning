using System;
using System.Collections.Generic;
using System.Text;

namespace Learning.Utils.Common
{
    public class CommonRegularExpressionClr
    {
        public const string AlphaNumericOnly = @"^[a-zA-Z0-9]+$";
        public const string PasswordStrength = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).{8,}$";
        public const string DateFormat = @"^(0[1-9]|1[0-2])/(0[1-9]|[12][0-9]|3[01])/\d{4}$";
        public const string Email = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public const string NumberOnly = @"^\d+$";
        public const string FloatingNumber = @"^\d+(\.\d+)?$";
        public const string CreditCard = @"^\d{13,16}$";
        public const string AlphaNumericWithSpecial = @"^[a-zA-Z0-9.\s,\/.,\\'!@#?\$%\^\&*\)\(+=._-]*$";

    }
}
