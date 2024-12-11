using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Sirbu_Iulia_Proiect_Restaurante.Data;
using Sirbu_Iulia_Proiect_Restaurante.Models;


namespace Sirbu_Iulia_Proiect_Restaurante.Controllers
{
    [Authorize(Policy = "AdministrativManager")]
    public class CustomersController : Controller
    {
        private readonly LibraryContext _context;
        private readonly string _baseUrl = "https://localhost:7282/api/Customers";

        public CustomersController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_baseUrl);
                if (response.IsSuccessStatusCode)
                {
                    var customers = JsonConvert.DeserializeObject<List<Customer>>(
                        await response.Content.ReadAsStringAsync()
                    );
                    return View(customers);
                }
            }
            return NotFound();
        }

        // GET: Customers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var customer = JsonConvert.DeserializeObject<Customer>(
                        await response.Content.ReadAsStringAsync()
                    );
                    return View(customer);
                }
            }
            return NotFound();
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("CustomerID,Name,Adress,BirthDate")] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            try
            {
                using (var client = new HttpClient())
                {
                    string json = JsonConvert.SerializeObject(customer);
                    var response = await client.PostAsync(_baseUrl,
                        new StringContent(json, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create record: {ex.Message}");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var customer = JsonConvert.DeserializeObject<Customer>(
                        await response.Content.ReadAsStringAsync()
                    );
                    return View(customer);
                }
            }

            return NotFound();
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("CustomerID,Name,Adress,BirthDate")] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            using (var client = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(customer);
                var response = await client.PutAsync($"{_baseUrl}/{customer.CustomerID}",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var customer = JsonConvert.DeserializeObject<Customer>(
                        await response.Content.ReadAsStringAsync()
                    );
                    return View(customer);
                }
            }

            return NotFound();
        }

        // POST: Customers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind("CustomerID")] Customer customer)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{customer.CustomerID}")
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(customer),
                            Encoding.UTF8, "application/json")
                    };
                    await client.SendAsync(request);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record: {ex.Message}");
            }

            return View(customer);
        }
    }
}
