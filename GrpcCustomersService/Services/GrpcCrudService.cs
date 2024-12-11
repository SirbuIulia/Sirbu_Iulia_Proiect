using Grpc.Core;
using GrpcCustomersService;
using DataAccess = Sirbu_Iulia_Proiect_Restaurante.Data;
using ModelAccess = Sirbu_Iulia_Proiect_Restaurante.Models;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcCustomersService.Services
{
    public class GrpcCrudService : CustomerService.CustomerServiceBase
    {
        private readonly DataAccess.LibraryContext db;

        public GrpcCrudService(DataAccess.LibraryContext db)
        {
            this.db = db;
        }

        public override Task<CustomerList> GetAll(Empty request, ServerCallContext context)
        {
            var customerList = new CustomerList();
            var query = from cust in db.Customer
                        select new Customer
                        {
                            CustomerId = cust.CustomerID,
                            Name = cust.Name,
                            Email = cust.Email,
                            Phone = cust.Phone
                        };

            customerList.Item.AddRange(query.ToArray());
            return Task.FromResult(customerList);
        }

        public override Task<Customer> Get(CustomerId request, ServerCallContext context)
        {
            var cust = db.Customer.FirstOrDefault(c => c.CustomerID == request.Id);
            if (cust == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
            }

            return Task.FromResult(new Customer
            {
                CustomerId = cust.CustomerID,
                Name = cust.Name,
                Email = cust.Email,
                Phone = cust.Phone
            });
        }

        public override Task<Empty> Insert(Customer request, ServerCallContext context)
        {
            var newCustomer = new ModelAccess.Customer
            {
               
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone
            };

            db.Customer.Add(newCustomer);
            db.SaveChanges();

            return Task.FromResult(new Empty());
        }


        public override Task<Customer> Update(Customer request, ServerCallContext context)
        {
            var cust = db.Customer.FirstOrDefault(c => c.CustomerID == request.CustomerId);
            if (cust == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
            }

            cust.Name = request.Name;
            cust.Email = request.Email;
            cust.Phone = request.Phone;
            db.SaveChanges();

            return Task.FromResult(new Customer
            {
                CustomerId = cust.CustomerID,
                Name = cust.Name,
                Email = cust.Email,
                Phone = cust.Phone
            });
        }

        public override Task<Empty> Delete(CustomerId request, ServerCallContext context)
        {
            var cust = db.Customer.FirstOrDefault(c => c.CustomerID == request.Id);
            if (cust == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
            }

            db.Customer.Remove(cust);
            db.SaveChanges();

            return Task.FromResult(new Empty());
        }
    }
}
