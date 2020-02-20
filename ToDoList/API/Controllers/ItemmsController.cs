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
    public class ItemmsController : ControllerBase
    {
        public IConfiguration _configuration;
        private IItemmService _itemmService;

        public ItemmsController(IConfiguration config, IItemmService itemmService)
        {
            _configuration = config;
            _itemmService = itemmService;
        }
        [HttpGet]
        public Task<IEnumerable<ItemmVM>> Get()
        {
            return _itemmService.Get();
        }

        [HttpGet("{Id}")]
        public Task<IEnumerable<ItemmVM>> Get(int id)
        {
            return _itemmService.Get(id);
        }

        [HttpPost]
        public IActionResult Post(ItemmVM itemmVM)
        {
            var push = _itemmService.Create(itemmVM);
            if (push > 0)
            {
                return Ok(push);
            }
            return BadRequest("Added Items Failed!");
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, ItemmVM itemmVM)
        {
            var put = _itemmService.Update(id, itemmVM);
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
            var delete = _itemmService.Delete(id);
            if (delete > 0)
            {
                return Ok(delete);
            }
            return BadRequest("Delete Failed!");
        }

        [HttpGet]
        [Route("PageSearch")]
        public async Task<ItemmVM> PageSearch(string keyword, int pageSize, int pageNumber)
        {
            //keyword = keyword.Substring(3);
            if (keyword == null)
            {
                keyword = "";
            }
            return await _itemmService.PageSearch(keyword, pageSize, pageNumber);
        }
    }
}