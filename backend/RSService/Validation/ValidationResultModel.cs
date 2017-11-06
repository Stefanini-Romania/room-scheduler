using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Validation
{
    public class ValidationResultModel
    {

        public string Message { get; set; }

        public List<ValidationError> Errors { get; set; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Message = "Could not detect error";
            //modelState.GetType();
            Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
            if (Errors.Count() != 0)
            {

                var errMsg = Errors.First().ErrorCode.ToString();
                if (errMsg.Contains("EventEdit"))
                {
                    Message = GeneralMessages.EventEdit;
                }
                else if (errMsg.Contains("Event"))
                {
                    Message = GeneralMessages.Event;
                }
                else if (errMsg.Contains("Auth"))
                {
                    Message = GeneralMessages.Authentication;
                }
            }

        }


    }
}
