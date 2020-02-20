using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Service.Interface;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class SuppsController : ControllerBase
    {
        public IConfiguration _configuration;
        private ISuppService _suppsService;

        public SuppsController(IConfiguration config, ISuppService suppsService)
        {
            _configuration = config;
            _suppsService = suppsService;
        }
        [HttpGet]
        public IEnumerable<Supp> Get()
        {
            return _suppsService.Get();
        }

        [HttpGet("{Id}")]
        public Task<IEnumerable<Supp>> Get(int id)
        {
            return _suppsService.Get(id);
        }

        [HttpPost]
        public IActionResult Post(SuppVM suppVM)
        {
            var push = _suppsService.Create(suppVM);
            if (push > 0)
            {
                return Ok(push);
            }
            return BadRequest("Added Suppliers Failed!");
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, SuppVM suppVM)
        {
            var put = _suppsService.Update(id, suppVM);
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
            var delete = _suppsService.Delete(id);
            if (delete > 0)
            {
                return Ok(delete);
            }
            return BadRequest("Delete Failed!");
        }

        [HttpGet]
        [Route("PageSearch")]
        public async Task<SuppVM> PageSearch(string keyword, int pageSize, int pageNumber)
        {
            //keyword = keyword.Substring(3);
            if (keyword == null)
            {
                keyword = "";
            }
            return await _suppsService.PageSearch(keyword, pageSize, pageNumber);
        }
    }
}