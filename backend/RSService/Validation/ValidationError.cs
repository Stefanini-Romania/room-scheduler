using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Validation
{

    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }
        public string ErrorCode { get; }

        public ValidationError(string field, string errorCode)
        {
            Field = field != string.Empty ? field : null;
            ErrorCode = errorCode;
        }
    }
}
