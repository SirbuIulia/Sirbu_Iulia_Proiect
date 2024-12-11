using Grpc.Net.Client;
using GrpcCustomersService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sirbu_Iulia_Proiect_Restaurante.Models;

namespace Sirbu_Iulia_Proiect_Restaurante.Controllers
{
    [Authorize(Policy = "AdministrativManager")]
    public class CustomersGrpcController : Controller
    {
        private readonly GrpcChannel channel;

        public CustomersGrpcController()
        {
            channel = GrpcChannel.ForAddress("https://localhost:7080");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var client = new CustomerService.CustomerServiceClient(channel);
            CustomerList cust = client.GetAll(new Empty());
            return View(cust);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Sirbu_Iulia_Proiect_Restaurante.Models.Customer customer)
        {
            if (ModelState.IsValid)
            {
                var client = new CustomerService.CustomerServiceClient(channel);

                var grpcCustomer = new GrpcCustomersService.Customer
                {
                    CustomerId = customer.CustomerID,
                    Name = customer.Name,
                    Email = customer.Email,
                    Phone = customer.Phone
                };

                client.Insert(grpcCustomer);

                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = new CustomerService.CustomerServiceClient(channel);
            GrpcCustomersService.Customer customer = client.Get(new CustomerId() { Id = (int)id });
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var client = new CustomerService.CustomerServiceClient(channel);
            client.Delete(new CustomerId() { Id = id });
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = new CustomerService.CustomerServiceClient(channel);
            GrpcCustomersService.Customer customer = client.Get(new CustomerId() { Id = (int)id });
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult Edit(int id, GrpcCustomersService.Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var client = new CustomerService.CustomerServiceClient(channel);
                client.Update(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = new CustomerService.CustomerServiceClient(channel);
            var customer = client.Get(new CustomerId { Id = id.Value });

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
    }
}
