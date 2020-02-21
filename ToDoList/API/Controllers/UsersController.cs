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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public IConfiguration _configuration;
        private IUserService _userService;

        public UsersController(
            IConfiguration config,
            IUserService userService,
            UserManager<User> userManager,
            SignInManager<User> signInManager
            )
        {
            _configuration = config;
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
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
        public async Task<IActionResult> Get(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userVM.Username, userVM.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(userVM.Username);
                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: signIn
                            );
                        var idtoken = new JwtSecurityTokenHandler().WriteToken(token);
                        claims.Add(new Claim("TokenSecurity", idtoken.ToString()));
                        return Ok(idtoken + "..." + user.Email);
                    }
                }
                return BadRequest(new { message = "Username or Password is Invalid" });
            }
            else
            {
                return BadRequest("Failed");
            }
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Log Out Success");
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User { };
                    user.Id = Guid.NewGuid().ToString();
                    user.UserName = userVM.Username;
                    user.Email = userVM.Email;
                    user.PasswordHash = userVM.Password;
                    user.CreateDate = DateTime.Now;

                    var result = await _userManager.CreateAsync(user, userVM.Password);
                    //var result = await _userService.Register(userVM);
                    if (result.Succeeded)
                    {
                        return Ok("Register succes");
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                //AddErrors(result);
            }

            return BadRequest(ModelState);
        }
        //public IActionResult Get(UserVM userVM)
        //{
        //    var get = _userService.Get(userVM);
        //    if (get != null)
        //    {
        //        var claims = new[] {
        //            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        //            new Claim("Id", get.Id.ToString()),
        //            new Claim("Username", get.Username),
        //            new Claim("Password", get.Password)
        //           };

        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        //        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddHours(1), signingCredentials: signIn);

        //        return Ok(new JwtSecurityTokenHandler().WriteToken(token) + "..." + get.Id);
        //        //return Ok(get);
        //    }
        //    return BadRequest("Login Failed!");
        //}

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