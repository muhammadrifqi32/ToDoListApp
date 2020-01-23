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
                    HttpContext.Session.SetString("id", user.Id.ToString());
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
    }
}