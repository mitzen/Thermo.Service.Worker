using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Thermo.Web.WebApi.Model;

namespace Thermo.Web.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SmtpSettingsController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<SmtpSettingsController> _logger;

        public SmtpSettingsController(ILogger<SmtpSettingsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //var users = _userService.GetAll();
            //var model = _mapper.Map<IList<UserModel>>(users);
            //return Ok(model);
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            //var user = _userService.GetById(id);
            //var model = _mapper.Map<UserModel>(user);
            //return Ok(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateModel model)
        {
            // map model to entity and set id
            //var user = _mapper.Map<User>(model);
            //user.Id = id;

            try
            {
                // update user 
                // _userService.Update(user, model.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //   _userService.Delete(id);
            return Ok();
        }
    }
}
