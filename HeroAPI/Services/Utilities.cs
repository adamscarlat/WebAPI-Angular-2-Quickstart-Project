using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HeroAPI.Services
{
    public class Utilities
    {
        public static Dictionary<string, List<string>> CreateFieldErrorDictionary(ModelStateDictionary modelState)
        {
            var fieldsErrors = new Dictionary<string, List<string>>();
            foreach(var key in modelState.Keys)
            {
                var errorList = new List<string>();
                modelState[key].Errors.ToList().ForEach(e => errorList.Add(e.ErrorMessage));
                
                fieldsErrors.Add(key, errorList);
            }
            return fieldsErrors;
        }

        public static object CreateJsonErrorResponse(Dictionary<string, List<string>> fieldErrors)
        {
            return new {
                FieldErrors = fieldErrors,
                IsSuccess = false
            };
        }

        public static object CreateJsonSuccessReponse(object responseObject)
        {
            return new {
                ResponseObject = responseObject,
                IsSuccess = true
            };
        }
    }
}