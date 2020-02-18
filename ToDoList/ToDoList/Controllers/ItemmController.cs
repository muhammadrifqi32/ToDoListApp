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
    public class ItemmController : Controller
    {
        readonly HttpClient client = new HttpClient();
        public ItemmController()
        {
            client.BaseAddress = new Uri("https://localhost:44377/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult Index()
        {
            var Id = HttpContext.Session.GetString("id");
            if (Id != null)
            {
                return View(List());
            }
            return RedirectToAction("Login", "User");
        }

        public async Task<IActionResult> List()
        {
            IEnumerable<ItemmVM> itemm = null;
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var responseTask = await client.GetAsync("Itemms");
            if (responseTask.IsSuccessStatusCode)
            {
                var readTask = await responseTask.Content.ReadAsAsync<IList<ItemmVM>>();
                return Ok(new { data = readTask });
            }
            else
            {
                itemm = Enumerable.Empty<ItemmVM>();
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }
            return Json(itemm);
        }

        public JsonResult InsertOrUpdate(ItemmVM itemmVM)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var myContent = JsonConvert.SerializeObject(itemmVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (itemmVM.Id == 0)
            {
                var result = client.PostAsync("itemms", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("itemms/" + itemmVM.Id, byteContent).Result;
                return Json(result);
            }
        }

        public async Task<JsonResult> GetById(int id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            HttpResponseMessage response = await client.GetAsync("itemms");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<ItemmVM>>();
                var itemm = data.FirstOrDefault(t => t.Id == id);
                var json = JsonConvert.SerializeObject(itemm, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
                return Json(json);
            }
            return Json("internal server error");
        }

        public JsonResult Delete(int id)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var result = client.DeleteAsync("itemms/" + id).Result;
            return Json(result);
        }
    }
}