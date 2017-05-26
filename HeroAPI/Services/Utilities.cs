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

            return CreateFieldErrorDictionary(modelState, fieldsErrors);

        }

        public static Dictionary<string, List<string>> CreateFieldErrorDictionary(ModelStateDictionary modelState, Dictionary<string, List<string>> fieldsErrors)
        {
            if (fieldsErrors == null)
                return null;

            foreach(var key in modelState.Keys)
            {
                var errorList = new List<string>();
                modelState[key].Errors.ToList().ForEach(e => errorList.Add(e.ErrorMessage));
                
                fieldsErrors.Add(key, errorList);
            }
            return fieldsErrors;
        }

        public static JsonResult CreateJsonErrorResponse(Dictionary<string, List<string>> fieldErrors)
        {
            var payload = new {
                FieldErrors = fieldErrors,
                IsSuccess = "false"
            };

            JsonResult jsonResult = new JsonResult(payload);

            return jsonResult;

            //return await Task.Factory.StartNew(() => JsonConvert.SerializeObject(payload));
        }

        public static JsonResult CreateJsonSuccessReponse(object responseObject, string redirectUrl = "")
        {
            var payload = new {
                ResponseObject = responseObject,
                IsSuccess = "true",
                RedirectUrl = redirectUrl
            };

            JsonResult jsonResult = new JsonResult(payload);
            
            return jsonResult;

            //return await Task.Factory.StartNew(() => JsonConvert.SerializeObject(payload));

        }
    }
}