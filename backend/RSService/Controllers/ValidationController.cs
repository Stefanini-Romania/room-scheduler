using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RSService.Validation;
using Microsoft.AspNetCore.Http;
using RSRepository;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RSService.Controllers
{
    
    public abstract class ValidationController : Controller
    {
        public IActionResult ValidationError(string message)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ValidationResultModel(message, ModelState));
        }
    }
}
