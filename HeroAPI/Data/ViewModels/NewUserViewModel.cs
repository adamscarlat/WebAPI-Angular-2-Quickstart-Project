using System.ComponentModel.DataAnnotations;
using HeroAPI.Services;
using Newtonsoft.Json;
using ViewModels.ViewModelValidation;

namespace ViewModels
{
    public class NewUserViewModel
    {
        [JsonProperty("username")]
        [Required(ErrorMessage = ResourceMaster.UsernameRequriedFieldError)] 
        [MinLength(3, ErrorMessage = ResourceMaster.UsernameRequiredLengthError)]
        public string Username { get; set; }

        [JsonProperty("password")]
        [Required(ErrorMessage = ResourceMaster.PasswordRequiredFieldError)] 
        [ValidatPasswordAttribute(ErrorMessage = ResourceMaster.PasswordTooSimpleError)]
        public string Password { get; set; }

        [JsonProperty("email")]
        [Required(ErrorMessage = ResourceMaster.EmailRequiredFieldError)]
        [EmailAddress(ErrorMessage = ResourceMaster.InvalidEmailAddressError)]
        public string Email { get; set; }

        // [JsonProperty("firstname")]
        // public string FirstName { get; set; }

        // [JsonProperty("lastname")]
        // public string LastName { get; set; }

        // [JsonProperty("dob")]
        // public DateTime DateOfBirth { get; set; }
    }
}