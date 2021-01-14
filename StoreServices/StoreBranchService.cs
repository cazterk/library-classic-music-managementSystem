using Microsoft.EntityFrameworkCore;
using StoreData;
using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreServices
{
    public class StoreBranchService : IStoreBranch
    {
        private StoreContext _context;

        public StoreBranchService(StoreContext context)
        {
            _context = context;
        }
        public void Add(StoreBranch newBranch)
        {
            _context.Add(newBranch);
            _context.SaveChanges();
        }

        public StoreBranch Get(int branchId)
        {
            return Getall()
                .FirstOrDefault(b => b.Id == branchId);
        }

        public IEnumerable<StoreBranch> Getall()
        {
            return _context.StoreBranches
                .Include(b => b.Customers)
                .Include(b => b.StoreAssets);
        }

        public IEnumerable<StoreAsset> GetAssets(int branchId)
        {
            return _context
                .StoreBranches
                .Include(b => b.StoreAssets)
                .FirstOrDefault(b => b.Id == branchId)
                .StoreAssets;
        }

        public IEnumerable<string> GetBranchHours(int branchId)
        {
            var hours = _context.BranchHours
                .Where(h => h.Branch.Id == branchId);
            return DateHelpers.HumanizeBizHours(hours);
        }

        public IEnumerable<Customer> GetCustomers(int branchId)
        {
            return _context.StoreBranches
                 .Include(b => b.Customers)
                 .FirstOrDefault(b => b.Id == branchId)
                 .Customers;
        }

        public bool IsBranchOpen(int branchId)
        {
            var currentTimeHour = DateTime.Now.Hour;
            var currentDayOfWeek = (int)DateTime.Now.DayOfWeek + 1;
            var hours = _context.BranchHours.Where(h => h.Branch.Id == branchId);
            var daysHours = hours.FirstOrDefault(h => h.DayOfTheWeek == currentDayOfWeek);

            return currentTimeHour < daysHours.CloseTime && currentTimeHour > daysHours.OpenTime;
          
        }
    }
    
    
}
