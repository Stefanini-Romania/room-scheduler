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
    
    public abstract class BaseController : Controller
    {
        private int _currentUser;

        protected RoomPlannerDevContext Context { get; }

        public BaseController()
        {
            var connection = @"Server=BUSWGVMINDEV3\MSSQLSERVER12;Database=RoomPlannerDev;User Id=roomplanner;Password=roomplanner123";

            var builder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<RoomPlannerDevContext>();
            builder.UseSqlServer(connection);

            Context = new RoomPlannerDevContext(builder.Options);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }

            base.Dispose(disposing);
        }

        public IActionResult ValidationError(string message)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ValidationResultModel(message, ModelState));
        }
    }
}
