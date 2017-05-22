using System;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.ViewModelValidation
{
    /// <summary>
    /// HeroViewModel custom validation.
    /// Example for custom validation attr. place as attribute above property in VM
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateNewUserAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var newUserVM = validationContext.ObjectInstance as NewUserViewModel;
           
            if (newUserVM == null)
               return new ValidationResult("Error- got no data to work with");
            
            if (!IsUsernameValid(newUserVM.Username))
                return new ValidationResult("username cannot be Adam");
            
            
            return ValidationResult.Success;
        }

        private bool IsUsernameValid(string username)
        {
            if (username == "Adam")
                return false;
            
            return true;
        }
    }
}
