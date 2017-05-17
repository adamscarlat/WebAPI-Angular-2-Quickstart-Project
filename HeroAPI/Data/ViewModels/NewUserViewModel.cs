using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ViewModels.ViewModelValidation;

namespace ViewModels
{
    public class NewUserViewModel
    {
        [JsonProperty("username")]
        [Required(ErrorMessage = "Username is a required field")] 
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
        public string Username { get; set; }

        [JsonProperty("password")]
        [Required(ErrorMessage = "Password is a required field")] 
        [ValidatPasswordAttribute(ErrorMessage = "Password must be 8 characters, at least 1 capital, at least 1 number, at least 1 lowercase and at least 1 special character")]
        public string Password { get; set; }

        // [JsonProperty("email")]
        // public string Email { get; set; }

        // [JsonProperty("firstname")]
        // public string FirstName { get; set; }

        // [JsonProperty("lastname")]
        // public string LastName { get; set; }

        // [JsonProperty("dob")]
        // public DateTime DateOfBirth { get; set; }
    }
}