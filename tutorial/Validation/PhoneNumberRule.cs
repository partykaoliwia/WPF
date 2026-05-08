using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ContactManager.Validation
{
    public class PhoneNumberRule : ValidationRule, IValidation
    {
        public string Name => "PHONE NUMBER FORMAT";
        public string Description => "Numer telefonu musi być w formacie XXX-XXX-XXX";
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? input = value as string;
            if(string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult(false, "Numer telefonu nie może być pusty");
            }
            string phonePattern = @"^\d{3}-\d{3}-\d{3}$";
            if(!Regex.IsMatch(input, phonePattern))
            {
                return new ValidationResult(false, Description);
            }
            return ValidationResult.ValidResult;
        }
    }
}
