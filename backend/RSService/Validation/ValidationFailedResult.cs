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
        public ValidationFailedResult(ModelStateDictionary modelState)
        : base(new ValidationResultModel(modelState))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
