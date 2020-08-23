using System;
using Microsoft.AspNetCore.Mvc;

namespace Thermo.Web.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = " Current time " + DateTime.Now;
            if (result != null)
                return Ok(result);
            return NotFound();
        }

    }
}
