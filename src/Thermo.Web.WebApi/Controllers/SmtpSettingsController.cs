using System;
using System.Threading.Tasks;
using AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Thermo.Web.WebApi.Model;
using Thermo.Web.WebApi.Model.SMTPModel;

namespace Thermo.Web.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SmtpSettingsController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly ThermoDataContext _thermoDataContext;
        private readonly SMTPSettingDataService _smtpDataService;
        public SmtpSettingsController(IOptions<AppSettings> appSettings, ThermoDataContext thermoDataContext)
        {
            _appSettings = appSettings.Value;
            _thermoDataContext = thermoDataContext;
            _smtpDataService = new SMTPSettingDataService(thermoDataContext);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewSmtpSetting([FromBody] NewSMTPRequest model)
        {
            try
            {
                var result = await _smtpDataService.SaveSmtpSettings(model);
                if (result > 0)
                    return Ok(result);

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _smtpDataService.GetAllSmtpSettings();
            if (result != null)
                return Ok(result);
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _smtpDataService.GetSmtpSettingsByCompanyIdAsync(id);
            if (result != null)
                return Ok(result);
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] NewSMTPRequest model)
        {
            try
            {
                var result = await _smtpDataService.UpdateSmtpSettings(model);
                
                if (result > 0)
                    return Ok(result);

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(SMTPDeleteRequest deleteRequest)
        {
            var result = await _smtpDataService.DeleteSmtpSettingsAsync(deleteRequest);
            if (result > 0)
                return Ok(result);
            return NotFound();
        }
    }
}
