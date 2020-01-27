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

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListsController : ControllerBase
    {
        private IToDoListService _todolistService;

        public ToDoListsController(IToDoListService todolistService)
        {
            _todolistService = todolistService;
        }

        [HttpGet]
        public Task<IEnumerable<ToDoListVM>> Get()
        {
            return _todolistService.Get();
            //return new string[] { "value1", "value2" };
        }

        // GET: api/ToDoLists/5
        [HttpGet("{Id}")]
        public Task<IEnumerable<ToDoListVM>> Get(int id)
        {
            return _todolistService.Get(id);
        }

        [HttpPost]
        public IActionResult Post(ToDoListVM todolistVM)
        {
            var push = _todolistService.Create(todolistVM);
            if (push > 0)
            {
                return Ok(push);
            }
            return BadRequest("Added Row Failed!");
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, ToDoListVM todolistVM)
        {
            var put = _todolistService.Update(id, todolistVM);
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
            var delete = _todolistService.Delete(id);
            if (delete > 0)
            {
                return Ok(delete);
            }
            return BadRequest("Delete Failed!");
        }
    }
}