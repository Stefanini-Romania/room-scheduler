using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.DTO
{
    public class EditExceptionDto : AvailabilityExceptionDto
    {
        public int Status { get; set; }
    }
}
