    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Service.Interface;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
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
                return Ok(get);
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