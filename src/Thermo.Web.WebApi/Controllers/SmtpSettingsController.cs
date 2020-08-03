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
        private readonly SMTPSettingDataService _personDataService;
        public SmtpSettingsController(IOptions<AppSettings> appSettings, ThermoDataContext thermoDataContext)
        {
            _appSettings = appSettings.Value;
            _thermoDataContext = thermoDataContext;
            _personDataService = new SMTPSettingDataService(thermoDataContext);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewSmtpSetting([FromBody] NewSMTPRequest model)
        {
            try
            {
                var result = await _personDataService.SaveSmtpSettings(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _personDataService.GetAllSmtpSettings();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _personDataService.GetSmtpSettingsByCompanyIdAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] NewSMTPRequest model)
        {
            try
            {
                var result = await _personDataService.SaveSmtpSettings(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(SMTPDeleteRequest deleteRequest)
        {
            var result = await _personDataService.DeleteSmtpSettingsAsync(deleteRequest);
            if (result != null)
                return Ok(result);
            return NotFound();
        }
    }
}
