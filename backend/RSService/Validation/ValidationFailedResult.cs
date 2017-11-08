using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Validation
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(string message, ModelStateDictionary modelState)
        : base(new ValidationResultModel(message, modelState))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
