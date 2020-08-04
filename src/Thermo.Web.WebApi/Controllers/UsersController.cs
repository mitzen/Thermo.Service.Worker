using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Thermo.Web.WebApi.Model;
using Thermo.Web.WebApi.Model.UserModel;

namespace Thermo.Web.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private const string DefaultUserRole = "user";
        private const int DefaultTokenExpirationDays = 7;
        private readonly AppSettings _appSettings;
        private readonly ThermoDataContext _thermoDataContext;
        private readonly PersonDataService _personDataService;
        public UsersController(IOptions<AppSettings> appSettings, ThermoDataContext thermoDataContext)
        {
            _appSettings = appSettings.Value;
            _thermoDataContext = thermoDataContext;
            _personDataService = new PersonDataService(thermoDataContext);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserAuthenticateRequest model)
        {
            if (model == null)
                return BadRequest();

            var user = _personDataService.Authenticate(model);
            if (user != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, (user.Role ?? DefaultUserRole))
                    }),
                    Expires = DateTime.UtcNow.AddDays(DefaultTokenExpirationDays),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info and authentication token
                return Ok(new
                {
                    Id = user.Nid,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    Token = tokenString
                });
            }

            return NotFound();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserNewRequest model)
        {
            try
            {
                var result = await _personDataService.RegisterUserAsync(model);
                if (result > 0)
                    return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _personDataService.GetUsersAsync();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _personDataService.GetUserByIdAsync(id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequest model)
        {
            try
            {
                var result = await _personDataService.UpdateUserAsync(model);
                if (result > 0)
                    return Ok(result);
              
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
              
            return BadRequest();
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete(UserDeleteRequest deleteRequest)
        {
            var result = await _personDataService.DeleteUserAsync(deleteRequest);
            if (result > 0)
                return Ok(result);
            return NotFound();
        }
    }
}
