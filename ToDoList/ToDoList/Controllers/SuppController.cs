using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace ToDoList.Controllers
{
    public class SuppController : Controller
    {
        readonly HttpClient client = new HttpClient();
        public SuppController()
        {
            client.BaseAddress = new Uri("https://localhost:44377/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Supp
        public IActionResult Index()
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
            IEnumerable<Supp> supp = null;
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var responseTask = await client.GetAsync("Supps");
            if (responseTask.IsSuccessStatusCode)
            {
                var readTask = await responseTask.Content.ReadAsAsync<IList<Supp>>();
                return Ok(new { data = readTask });
            }
            else
            {
                supp = Enumerable.Empty<Supp>();
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }
            return Json(supp);
        }

        public JsonResult LoadSupplier()
        {
            IEnumerable<Supp> supp = null;

            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var responseTask = client.GetAsync("Supps");
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Supp>>();
                readTask.Wait();
                supp = readTask.Result;
            }
            else
            {
                supp = Enumerable.Empty<Supp>();
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(supp);
        }

        public async Task<SuppVM> Paging(int pageSize, int pageNumber, string keyword)
        {
            try
            {
                client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
                var responseTask = await client.GetAsync("Supps/PageSearch?" + "keyword=" + keyword + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber);
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = await responseTask.Content.ReadAsAsync<SuppVM>();
                    return readTask;
                }

            }
            catch (Exception)
            {

            }
            return null;
        }

        [HttpGet("Supp/PageData/")]
        public IActionResult PageData(IDataTablesRequest request)
        {
            var pageSize = request.Length;
            var pageNumber = request.Start / request.Length + 1;
            var keyword = request.Search.Value;
            //var data = Search(keyword, status).Result;
            //var filteredData = data;
            var dataPage = Paging(pageSize, pageNumber, keyword).Result;
            var response = DataTablesResponse.Create(request, dataPage.length, dataPage.filterlength, dataPage.data);
            return new DataTablesJsonResult(response, true);
        }

        public JsonResult InsertOrUpdate(SuppVM suppVM)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var myContent = JsonConvert.SerializeObject(suppVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (suppVM.Id == 0)
            {
                var result = client.PostAsync("supps", byteContent).Result;
                return Json(result);
            }
            else
            {
                var result = client.PutAsync("supps/" + suppVM.Id, byteContent).Result;
                return Json(result);
            }
        }

        public async Task<JsonResult> GetById(int id)
        {
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            HttpResponseMessage response = await client.GetAsync("supps");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<SuppVM>>();
                var supp = data.FirstOrDefault(t => t.Id == id);
                var json = JsonConvert.SerializeObject(supp, Formatting.None, new JsonSerializerSettings()
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
            var result = client.DeleteAsync("supps/" + id).Result;
            return Json(result);
        }

        public async Task<IActionResult> exporttoExcel()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var columnHeaders = new String[]
            {
                "ID",
                "Supplier Name"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("List Supplier");
                using (var cells = worksheet.Cells[1, 1, 1, 2])
                {
                    cells.Style.Font.Bold = true;
                }

                for (var i = 0; i < columnHeaders.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = columnHeaders[i];
                }

                var j = 2;
                HttpResponseMessage response = await client.GetAsync("supps");
                if (response.IsSuccessStatusCode)
                {
                    var readTask = await response.Content.ReadAsAsync<IList<Supp>>();
                    foreach (var supplier in readTask)
                    {
                        worksheet.Cells["A" + j].Value = supplier.Id;
                        worksheet.Cells["B" + j].Value = supplier.Name;
                        j++;
                    }
                }
                result = package.GetAsByteArray();
            }
            return File(result, "application/ms-excel", $"Supplier{DateTime.Now.ToString("hh:mm:ss MM/dd/yyyy")}.xlsx");
        }

        public async Task<IActionResult> exporttoCSV()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44377/api/")
            };
            client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("JWToken"));
            var columnHeaders = new String[]
            {
                "ID",
                "Supplier Name"
            };
            byte[] buffer;
            var suppcsv = new StringBuilder();
            HttpResponseMessage response = await client.GetAsync("supps");
            if (response.IsSuccessStatusCode)
            {
                var readTask = await response.Content.ReadAsAsync<IList<Supp>>();
                var supp = (from supplier in readTask
                            select new object[]
                            {
                                            supplier.Id,
                                            $"\"{supplier.Name}\""
                            }).ToList();
                supp.ForEach(line =>
                {
                    suppcsv.AppendLine(string.Join(",", line));
                });
            }
            buffer = Encoding.ASCII.GetBytes($"{string.Join(",", columnHeaders)}\r\n{suppcsv.ToString()}");
            return File(buffer, "text/csv", $"Supplier{DateTime.Now.ToString("hh:mm:ss MM/dd/yyyy")}.csv");
        }

        //public ActionResult Report(Supp supp)
        //{
        //    SuppList suppList = new SuppList();
        //    byte[] abytes = suppList.PrepareReport();
        //    return File(abytes, "application/pdf");
        //}
    }
}