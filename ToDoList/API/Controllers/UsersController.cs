    using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Service.Interface;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public IConfiguration _configuration;
        private IUserService _userService;

        public UsersController(IConfiguration config,IUserService userService)
        {
            _configuration = config;
            _userService = userService;
        }

        [HttpGet]
        public Task<IEnumerable<User>> Get()
        {
            return _userService.Get();
            //return new string[] { "value1", "value2" };
        }

        // GET: api/Users/5
        [HttpGet("{Id}")]
        public Task<IEnumerable<User>> Get(int id)
        {
            return _userService.Get(id);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Get(UserVM userVM)
        {
            var get = _userService.Get(userVM);
            if (get != null)
            {
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", get.Id.ToString()),
                    new Claim("Username", get.Username),
                    new Claim("Password", get.Password)
                   };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddHours(1), signingCredentials: signIn);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token) + "..." + get.Id);
                //return Ok(get);
            }
            return BadRequest("Login Failed!");
        }

        // POST: api/Users
        [HttpPost]
        public IActionResult Post(UserVM userVM)
        {
            var push = _userService.Create(userVM);
            if (push > 0)
            {
                return Ok(push);
            }
            return BadRequest("Added Row Failed!");
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, UserVM userVM)
        {
            var put = _userService.Update(id, userVM);
            if (put > 0)
            {
                return Ok(put);
            }
            return BadRequest("Update Failed!");
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var delete = _userService.Delete(id);
            if (delete > 0)
            {
                return Ok(delete);
            }
            return BadRequest("Delete Failed!");
        }
    }
}