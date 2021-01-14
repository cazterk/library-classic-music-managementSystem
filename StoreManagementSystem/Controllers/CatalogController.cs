using StoreData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreManagementSystem.Models.Catalog;
using StoreManagementSystem.Models.CheckoutModel;

namespace StoreManagementSystem.Controllers
{
    public class CatalogController : Controller
    {
        private IStoreAsset _assets;
        private ICheckout _checkouts;

        public CatalogController(IStoreAsset assets, ICheckout checkouts)
        {
            _assets = assets;
            _checkouts = checkouts;
        }
        public IActionResult Index()
        {
            var assetModels = _assets.GetAll();

            var listingResult = assetModels
                .Select(a => new AssetIndexListingModel
                {
                    Id = a.Id,
                    ImageUrl = a.ImageUrl,
                    AuthorOrDirector = _assets.GetAuthorOrDirector(a.Id),
                    DeweyCalNumber = _assets.GetDeweyIndex(a.Id),
                    Title = _assets.GetTitle(a.Id),
                    Type = _assets.GetType(a.Id),
                    NumberOfCopies = _checkouts.GetNumberOfCopies(a.Id)
                    
                    
                }).ToList();

            var model = new AssetIndexModel
            {
                Assets = listingResult
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var asset = _assets.GetById(id);
            var currentHolds = _checkouts.GetCurrentHolds(id)
                .Select(a => new AssetHoldModel
                {
                    HoldPlaced = _checkouts.GetCurrentHoldPlaced(a.Id).ToString("d"),
                    CustomerName = _checkouts.GetCurrentHoldCustomerName(a.Id)

                });

            var model = new AssetDetailModel
            {
                AssetId = id,
                Title = asset.Title,
                Type = _assets.GetType(id),
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _assets.GetAuthorOrDirector(id),
                CurrentLocation = _assets.GetCurrentLocation(id).Name,
                DeweyCallNumber = _assets.GetDeweyIndex(id),
                ISBN = _assets.GetIsbn(id),
                CheckoutHistory = _checkouts.GetCheckoutHistory(id),
                LastCheckout = _checkouts.GetLatestCheckout(id),
                CustomerName = _checkouts.GetCurrentCheckoutCustomer(id),
                CurrentHolds = currentHolds

            };

            return View(model);
        }

        public IActionResult Checkout(int id)
        {
            var asset = _assets.GetById(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                StoreCardId ="",
                IsCheckedOut = _checkouts.IsCheckedOut(id)
            };
            return View(model);
        }

        public IActionResult CheckIn(int id)
        {
            _checkouts.CheckIntItem(id);
            return RedirectToAction("Detail", new { id = id });

        }

        public IActionResult Hold(int id)
        {
            var asset = _assets.GetById(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                StoreCardId = "",
                IsCheckedOut = _checkouts.IsCheckedOut(id),
                HoldCount = _checkouts.GetCurrentHolds(id).Count()
            };
            return View(model);
        }

        public IActionResult MarkLost(int assetId)
        {
            _checkouts.MarkLost(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        public IActionResult MarkFound(int assetId)
        {
            _checkouts.MarkFound(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceCheckout(int assetId, int StoreCardId)
        {
            _checkouts.CheckOutItem(assetId, StoreCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }
        [HttpPost]
        public IActionResult PlaceHold(int assetId, int StoreCardId)
        {
            _checkouts.PlaceHold(assetId, StoreCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }

    }

}
