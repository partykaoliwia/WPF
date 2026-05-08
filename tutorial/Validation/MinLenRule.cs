using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ContactManager.Validation
{
    public class MinLenRule : ValidationRule, IValidation
    {
        public string Name => "MINIMUM LENGTH";
        public string Description => "Wymagane minimum 5 znaków";
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? input = value as string;
            if(string.IsNullOrWhiteSpace(input) || input.Length<5)
            {
                return new ValidationResult(false, Description);
            }
            return ValidationResult.ValidResult;
        }
    }
}
