namespace HeroAPI.Services
{
    public class ResourceMaster
    {
        #region Application Errors

        public const string GeneralApplicationError = "An error occured, try again later";

        #endregion

        #region Model Error Messages

        public const string UsernameRequriedFieldError = "Username is a required field";
        public const string InvalidUsernameError = "Invalid username";

        public const string UsernameRequiredLengthError = "Username must be at least 3 characters";

        public const string PasswordRequiredFieldError = "Password is a required field";

        public const string PasswordTooSimpleError = "Password must be 8 characters, at least 1 capital, at least 1 number, at least 1 lowercase and at least 1 special character";

        public const string EmailRequiredFieldError = "Email is a required field";
        
        public const string InvalidEmailAddressError = "Invalid email address";


        #endregion
    }
}