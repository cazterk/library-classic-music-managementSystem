using Microsoft.AspNetCore.Mvc;
using StoreData;
using StoreManagementSystem.Models.BranchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagementSystem.Controllers
{
    public class BranchController : Controller
    {
        private IStoreBranch _branch;

        public BranchController(IStoreBranch branch)
        {
            _branch = branch;
        }
        public IActionResult Index()
        {
            var branches = _branch.Getall().Select(branch => new BranchDetailModel
            {
                Id = branch.Id,
                Name = branch.Name,
                IsOpen = _branch.IsBranchOpen(branch.Id),
                NumberOfAssets = _branch.GetAssets(branch.Id).Count(),
                NumberOfCustomers = _branch.GetCustomers(branch.Id).Count()

            });

            var model = new BranchIndexModel()
            {
                Branches = branches
            };
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var branch = _branch.Get(id);

            var model = new BranchDetailModel
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                Telephone = branch.Telephone,
                OpenDate = branch.OpenDate.ToString("yyyy-mm-dd"),
                NumberOfAssets = _branch.GetAssets(branch.Id).Count(),
                NumberOfCustomers = _branch.GetCustomers(branch.Id).Count(),
                TotalAssetValue = _branch.GetAssets(id).Sum(a => a.Cost),
                ImageUrl = branch.ImageUrl,
                HoursOpen = _branch.GetBranchHours(id)

            };

            return View(model);

        }
    }
}
