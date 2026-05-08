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
    public class EmailRule : ValidationRule, IValidation
    {
        public string Name => "CORRECT EMAIL";
        public string Description => "Wpisany tekst musi być adresem email";
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? input=value as string;
            if(string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult(false, "Adres nie może być pusty");
            }
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if(!Regex.IsMatch(input,emailPattern))
            {
                return new ValidationResult(false, Description);
            }
            return ValidationResult.ValidResult;
        }
    }
}
