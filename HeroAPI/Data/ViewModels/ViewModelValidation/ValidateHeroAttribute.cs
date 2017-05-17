using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ViewModels;

namespace ViewModels.ViewModelValidation
{
    /// <summary>
    /// HeroViewModel custom validation
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidateHeroAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var heroViewModel = value as HeroViewModel;
            if (heroViewModel == null)
                return false;
            
            if (!IsIdValid(heroViewModel.HeroId))
                return false;
            
            if (!IsNameValid(heroViewModel.HeroName))
                return false;
            
            return true;
        }

        /// <summary>
        /// Check that the id is a positive number
        /// between 0 to int.max
        /// </summary>
        /// <param name="id">id to check</param>
        /// <returns>true if id is valid, false otherwise</returns>
        private bool IsIdValid(int id)
        {
            if (id >= 0 && id < int.MaxValue)
                return true;
            
            return false;
        }

        /// <summary>
        /// Checks that name contains only letters
        /// </summary>
        /// <param name="name">name to check</param>
        /// <returns>true if name is valid, false otherwise</returns>
        private bool IsNameValid(string name)
        {
            if (name == null)
                return false;

            Regex re = new Regex("^[\\w_-]+$");
            return re.IsMatch(name);
        }
    }
}