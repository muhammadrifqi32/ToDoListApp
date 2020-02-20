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
    [Authorize(AuthenticationSchemes = "Bearer")]
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
        [HttpGet("{Id}/{status}")]
        public Task<IEnumerable<ToDoListVM>> Get(int id, int status)
        {
            return _todolistService.Get(id, status);
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

        [HttpPut]
        [Route("Checkedlist/{id}")]
        public IActionResult Checkedlist(int id, ToDoList toDoList)
        {
            var put = _todolistService.Checkedlist(id, toDoList);
            if (put > 0)
            {
                return Ok(put);
            }
            return BadRequest("Checked Failed!");
        }

        [HttpPut]
        [Route("Uncheckedlist/{id}")]
        public IActionResult Uncheckedlist(int id, ToDoList toDoList)
        {
            var put = _todolistService.Uncheckedlist(id, toDoList);
            if (put > 0)
            {
                return Ok(put);
            }
            return BadRequest("Unchecked Failed!");
        }

        //[HttpGet]
        //[Route("Search/{id}/{status}/{keyword}")]
        //public Task<IEnumerable<ToDoListVM>> Search(int id, int status, string keyword)
        //{
        //    keyword = keyword.Substring(3);
        //    return _todolistService.Search(id, status, keyword);
        //}

        //[HttpGet]
        //[Route("Paging/{id}/{status}/{keyword}/{pageSize}/{pageNumber}")]
        //public Task<IEnumerable<ToDoListVM>> Paging(int id, int status, string keyword, int pageSize, int pageNumber)
        //{
        //    keyword = keyword.Substring(3);
        //    return _todolistService.Paging(id, status, keyword, pageSize, pageNumber);
        //}
        
        [HttpGet]
        [Route("PageSearch")]
        public async Task<ToDoListVM> PageSearch(string id, int status, string keyword, int pageSize, int pageNumber)
        {
            //keyword = keyword.Substring(3);
            if (keyword == null)
            {
                keyword = "";
            }
            return await _todolistService.PageSearch(id, status, keyword, pageSize, pageNumber);
        }
    }
}