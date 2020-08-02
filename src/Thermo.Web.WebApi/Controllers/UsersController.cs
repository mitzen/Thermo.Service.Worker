﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            //var user = _userService.Authenticate(model.Username, model.Password);
            //if (user == null)

            var ps = new PersonDataService(_thermoDataContext);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "jeremy"),
                    new Claim(ClaimTypes.Role, "user")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = "user.Id",
                Username = "user.Username",
                FirstName = "user.FirstName",
                LastName = "user.LastName",
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserNewRequest model)
        {   
            try
            {
                var result = _personDataService.RegisterUserAsync(model);
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
        public IActionResult Update([FromBody] UserUpdateRequest model)
        {   
            try
            {
                var result = _personDataService.SaveUserAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete()]
        public IActionResult Delete(UserDeleteRequest deleteRequest)
        {
            var result = _personDataService.DeleteUserAsync(deleteRequest);
            if (result != null)
                return Ok(result);
            return NotFound();
        }
    }
}
