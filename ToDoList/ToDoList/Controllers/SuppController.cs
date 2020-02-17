using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult Index()
        {
            return View();
        }

        // GET: Supp/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Supp/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Supp/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Supp/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Supp/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Supp/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Supp/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}