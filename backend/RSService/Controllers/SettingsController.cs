using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RSData.Models;
using RSRepository;
using RSService.DTO;
using RSService.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSService.Controllers
{
    public class SettingsController : ValidationController
    {
        private ISettingsRepository _settingsRepository;
        private ILogger<SettingsController> _logger;
        private readonly RoomPlannerDevContext context;

        public SettingsController(RoomPlannerDevContext context, ILogger<SettingsController> logger)
        {
            this.context = context;
            _settingsRepository = new SettingsRepository(context);
            _logger = logger;
        }

        [HttpGet("/settings/list")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult GetSettings()
        {
            var results = _settingsRepository.GetSettings();

            if (results == null)
                return NotFound();

            List<SettingsDto> final_result = new List<SettingsDto>();

            foreach (var it in results)
            {
                final_result.Add(new SettingsDto()
                {
                    Id = it.Id,
                    VarName = it.VarName,
                    Value = it.Value
                });
            }

            return Ok(final_result);
        }

        [HttpPut("/settings/edit/{id}")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult UpdateVariable(int id, [FromBody] SettingsDto model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationError(GeneralMessages.Settings);
            }

            var setting = _settingsRepository.GetSettingsById(id);

            setting.Value = model.Value;

            context.SaveChanges();

            return Ok();
        }



    }
}
