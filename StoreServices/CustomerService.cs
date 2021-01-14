using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StoreData;
using StoreData.Models;
using System.Linq;

namespace StoreServices
{
    public class CustomerService : ICustomer
    {
        private StoreContext _context;

        public CustomerService(StoreContext context)
        {
            _context = context;
        }

        public void Add(Customer newCustomer)
        {
            _context.Add(newCustomer);
            _context.SaveChanges();
        }

        public Customer Get(int id)
        {
            return GetAll()
                 .FirstOrDefault(customer => customer.Id == id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers
                 .Include(customer => customer.CustomerCard)
                 .Include(customer => customer.HomeStoreBranch);
        
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int customerId)
        {
            var cardId = Get(customerId).CustomerCard.Id;

            return _context.CheckoutHistories
                .Include(co => co.StoreCard)
                .Include(co => co.StoreAsset)
                .Where(co => co.StoreCard.Id == cardId)
                .OrderByDescending(co => co.CheckedOut);
        }

        public IEnumerable<Checkout> GetCheckouts(int customerId)
        {
            var cardId = Get(customerId).CustomerCard.Id;
                
              
            return _context.Checkouts
                .Include(co => co.StoreCard)
                .Include(co => co.StoreAsset)
                .Where(co => co.StoreCard.Id == cardId);
        }

        public IEnumerable<Hold> GetHolds(int customerId)
        {
            var cardId = Get(customerId).CustomerCard.Id;

            return _context.Holds
                .Include(h => h.StoreCard)
                .Include(h => h.StoreAsset)
                .Where(h => h.StoreCard.Id == cardId)
                .OrderByDescending(h => h.HoldPlaced);
        }
    }
}
