using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ToDoList.Util;

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
                    //var data = result.Content.ReadAsAsync<User>();
                    //data.Wait();
                    //var user = data.Result;
                    //if (Hashing.ValidatePassword(userVM.Password, userVM.Password)) { 
                    var data = result.Content.ReadAsStringAsync().Result.Replace("\"", "").Split("...");
                    var token = "Bearer " + data[0];
                    var email = data[1];
                    HttpContext.Session.SetString("id", email);
                    HttpContext.Session.SetString("JWToken", token);
                    return RedirectToAction(nameof(Index));
                    //}
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        public IActionResult LogOut()
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));

            var result = client.GetAsync("Users/Logout").Result;
            if (result.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("id");
                HttpContext.Session.Remove("token");
                return RedirectToAction(nameof(Index));
            }
            return View();

        }

        [HttpGet]
        public ActionResult Register()
        {
            return View("Register");
        }


        [HttpPost]
        public ActionResult Register(UserVM userVM)
        {
            userVM.Password = Hashing.HashPassword(userVM.Password);
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            var myContent = JsonConvert.SerializeObject(userVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PostAsync("users", byteContent).Result;
            if (result.IsSuccessStatusCode)
            {
                //MailMessage mm = new MailMessage("muhammadrifqi0@gmail.com", userVM.Email);
                //mm.Subject = "[Password] " + DateTime.Now.ToString("ddMMyyyyhhmmss");
                //mm.Body = "Hi " + userVM.Username + "\nThis Is Your New Password : " + userVM.Password;

                //SmtpClient smtp = new SmtpClient();
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                //smtp.EnableSsl = true;

                //NetworkCredential nc = new NetworkCredential("muhammadrifqi0@gmail.com", "085376886737");
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = nc;
                //smtp.Send(mm);
                //ViewBag.Message = "Password Has Been Sent.Check your email to login";
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        //public ActionResult Logout()
        //{
        //    HttpContext.Session.Remove("id");
        //    return RedirectToAction(nameof(Index));
        //}

        //[HttpGet("user/list/{status}")]
        //public async Task<IActionResult> List(int status)
        //{
        //    IEnumerable<Data.Model.ToDoList> todolist = null;
        //    var client = new HttpClient
        //    {
        //        BaseAddress = new Uri("https://localhost:44377/api/")
        //    };
        //    client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
        //    //var userid = convert.toint32(httpcontext.session.getstring("id"));
        //    var responseTask = await client.GetAsync("ToDoLists/" + HttpContext.Session.GetString("id") + "/" + status);
        //    if (responseTask.IsSuccessStatusCode)
        //    {
        //        var readTask = await responseTask.Content.ReadAsAsync<IList<Data.Model.ToDoList>>();
        //        return Ok(new { data = readTask });
        //    }
        //    else
        //    {
        //        todolist = Enumerable.Empty<Data.Model.ToDoList>();
        //        ModelState.AddModelError(string.Empty, "server error, try after some time");
        //    }
        //    return Json(todolist);
        //}

        public JsonResult InsertOrUpdate(ToDoListVM toDoListVM)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            toDoListVM.UserId = HttpContext.Session.GetString("id");
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
        public async Task<JsonResult> GetById(int id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            HttpResponseMessage response = await client.GetAsync("todolists");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<Data.Model.ToDoList>>();
                var item = data.FirstOrDefault(t => t.Id == id);
                var json = JsonConvert.SerializeObject(item, Formatting.None, new JsonSerializerSettings()
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
            var result = client.DeleteAsync("todolists/" + id).Result;
            return Json(result);
        }
        public JsonResult CheckedTodoList(int Id, ToDoListVM toDoListVM)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var getEmail = HttpContext.Session.GetString("id");
            var myContent = JsonConvert.SerializeObject(toDoListVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PutAsync("todolists/Checkedlist/" + Id, byteContent).Result;
            if (result.IsSuccessStatusCode)
            {
                //MailMessage mm = new MailMessage("muhammadrifqi0@gmail.com", userVM.Email);
                //mm.Subject = "[Password] " + DateTime.Now.ToString("ddMMyyyyhhmmss");
                //mm.Body = "Hi " + userVM.Username + "\nThis Is Your New Password : " + userVM.Password;

                //SmtpClient smtp = new SmtpClient();
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                //smtp.EnableSsl = true;

                //NetworkCredential nc = new NetworkCredential("muhammadrifqi0@gmail.com", "085376886737");
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = nc;
                //smtp.Send(mm);
                //ViewBag.Message = "Password Has Been Sent.Check your email to login";
                return Json(result);
            }
            else
            {
                return Json("");
            }
        }
        public JsonResult UncheckedTodoList(int Id, Data.Model.ToDoList toDoList)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var myContent = JsonConvert.SerializeObject(toDoList);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PutAsync("todolists/Uncheckedlist/" + Id, byteContent).Result;
            return Json(result);
        }

        //public async Task<IEnumerable<ToDoListVM>> Search(string keyword, int status)
        //{
        //    try
        //    {
        //        client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
        //        //var userid = convert.toint32(httpcontext.session.getstring("id"));
        //        var responseTask = await client.GetAsync("ToDoLists/Search/" + HttpContext.Session.GetString("id") + "/" + status + "/" + keyword);
        //        if (responseTask.IsSuccessStatusCode)
        //        {
        //            var readTask = await responseTask.Content.ReadAsAsync<IList<ToDoListVM>>();
        //            return readTask;
        //        }

        //    }
        //    catch(Exception)
        //    {

        //    }
        //    return null;
        //}

        public async Task<ToDoListVM> Paging(int pageSize, int pageNumber, string keyword, int status)
        {
            try
            {
                client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
                var getsession = HttpContext.Session.GetString("id");
                var responseTask = await client.GetAsync("todolists/pagesearch?id=" + getsession + "&status=" + status + "&keyword=" + keyword + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = await responseTask.Content.ReadAsAsync<ToDoListVM>();
                    return readTask;
                }

            }
            catch (Exception)
            {

            }
            return null;
        }

        [HttpGet("user/pagedata/{status}")]
        public IActionResult PageData(int status, IDataTablesRequest request)
        {
            var pageSize = request.Length;
            var pageNumber = request.Start / request.Length + 1;
            var keyword = request.Search.Value;
            //var data = Search(keyword, status).Result;
            //var filteredData = data;
            var dataPage = Paging(pageSize, pageNumber, keyword, status).Result;
            var response = DataTablesResponse.Create(request, dataPage.length, dataPage.filterlength, dataPage.data);
            return new DataTablesJsonResult(response, true);
        }
    }
}