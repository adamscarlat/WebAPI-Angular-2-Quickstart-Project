using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ViewModels.ViewModelValidation
{
    /// <summary>
    /// Password custom validation.
    /// Custom validation attr. place as attribute above property in VM
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidatPasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var password = value as string;
            if (password == null)
                return false;
            
            return IsValidPassword(password);
        }

        private bool IsValidPassword(string password)
        {
            var pattern = "^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }
    }
}
