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
    public class SettingsController : BaseController
    {
        private ISettingsRepository _settingsRepository;
        private ILogger<SettingsController> _logger;

        public SettingsController(ILogger<SettingsController> logger)
        {
            _settingsRepository = new SettingsRepository(Context);
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
                    VarName = it.VarName,
                    Value = it.Value
                });
            }

            return Ok(final_result);
        }

        [HttpPut("/settings/session/edit/{value}")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult ChangeSessionTimeSpan(int value)
        {
            if (value == 0)
            {
                return ValidationError(GeneralMessages.Settings);
            }

            var sessionTimeSpan = _settingsRepository.GetSessionTimeSpan();
            sessionTimeSpan.Value = value.ToString();

            Context.SaveChanges();

            return Ok();
        }

        [HttpPut("/settings/edit/{varname}/{value}")]
        [Authorize(Roles = nameof(UserRoleEnum.admin))]
        public IActionResult ChangeSessionTimeSpan(string varname, string value)
        {
            if (varname == null || value == null)
            {
                return ValidationError(GeneralMessages.Settings);
            }

            var setting = _settingsRepository.GetSettingsByName(varname);

            setting.Value = value;

            Context.SaveChanges();

            return Ok();
        }



    }
}
