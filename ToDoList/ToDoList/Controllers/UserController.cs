using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ToDoList.Controllers
{
    public class UserController : Controller
    {
        readonly HttpClient client = new HttpClient();
        public UserController()
        {
            client.BaseAddress = new Uri("https://localhost:44377/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public IActionResult Index()
        {
            var Id = HttpContext.Session.GetString("id");
            if (Id != null)
            {
                return View();
            }
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login(UserVM userVM)
        {
            try
            {
                var myContent = JsonConvert.SerializeObject(userVM);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = client.PostAsync("Users/Login", byteContent).Result;
                if (result.IsSuccessStatusCode)
                {
                    var data = result.Content.ReadAsAsync<User>();
                    data.Wait();
                    var user = data.Result;
                    HttpContext.Session.SetInt32("id", user.Id);
                    //var Id = HttpContext.Session.GetString("Id");
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("id");
            return RedirectToAction(nameof(Index));
        }

        public JsonResult List()
        {
            IEnumerable<Data.Model.ToDoList> todolist = null;
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            var responseTask = client.GetAsync("todolists");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Data.Model.ToDoList>>();
                readTask.Wait();
                todolist = readTask.Result;
            }
            else
            {
                todolist = Enumerable.Empty<Data.Model.ToDoList>();
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }
            return Json(todolist);
        }
        public JsonResult InsertOrUpdate(ToDoListVM toDoListVM)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            var myContent = JsonConvert.SerializeObject(toDoListVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (toDoListVM.Id == 0)
            {
                var result = client.PostAsync("todolists", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("todolists/" + toDoListVM.Id, byteContent).Result;
                return Json(result);
            }
        }
    }
}