using Microsoft.AspNetCore.Mvc;
using StoreData;
using StoreData.Models;
using StoreManagementSystem.Models.CustomerModel;
using System.Collections.Generic;
using System.Linq;

namespace StoreManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomer _customer;
        
       public CustomerController(ICustomer customer)
        {
            _customer = customer;
        }
        public IActionResult Index()
        {
            var allCustomers = _customer.GetAll();

            var customerModels = allCustomers.Select(c => new CustomerDetailModel
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                StoreCardId = c.CustomerCard.Id,
                Overdue = c.CustomerCard.Fees,
                HomeStoreBranch = c.HomeStoreBranch.Name



            }) .ToList();

            var model = new CustomerIndexModel()
            {
                Customers = customerModels
            };
            return View(model);
        }
        public IActionResult Detail(int id)
        {
            var customer = _customer.Get(id);

            var model = new CustomerDetailModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                HomeStoreBranch = customer.HomeStoreBranch.Name,
                MemberSince = customer.CustomerCard.Created,
                Overdue = customer.CustomerCard.Fees,
                StoreCardId = customer.CustomerCard.Id,
                Telephone = customer.TelephoneNumber,
                AssetsCheckedOut = _customer.GetCheckouts(id).ToList() ?? new List<Checkout>(),
            CheckOutHistory = _customer.GetCheckoutHistory(id),
                Holds = _customer.GetHolds(id)



            };
            return View(model);
        }
    }
}
