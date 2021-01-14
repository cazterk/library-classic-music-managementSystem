using Microsoft.EntityFrameworkCore;
using StoreData;
using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreServices
{
    public class CheckoutService : ICheckout

    {
        private StoreContext _context;

        public CheckoutService(StoreContext context)
        {
            _context = context;
        }

        public void Add(Checkout newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }

        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        public Checkout GetById(int checkoutId)
        {
            return GetAll()
                .FirstOrDefault(checkout => checkout.Id == checkoutId);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            return _context.CheckoutHistories
                .Include(h => h.StoreAsset)
                .Include(h => h.StoreCard)
                .Where(h => h.StoreAsset.Id == id);
        }

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Holds
                .Include(h => h.StoreAsset)
                .Where(h => h.StoreAsset.Id == id);
        }
        public Checkout GetLatestCheckout(int assetId)
        {
            return _context.Checkouts
                .Where(c => c.StoreAsset.Id == assetId)
                .OrderByDescending(c => c.Since)
                .FirstOrDefault();

        }

        public void MarkFound(int assetId)
        {
            var now = DateTime.Now;


            UpdateAssetStatus(assetId, "Available");
            RemoveExistingCheckouts(assetId);
            CloseExistingCheckoutHistory(assetId, now);

            _context.SaveChanges();
        }

        private void UpdateAssetStatus(int assetId, string newStatus)
        {
            var item = _context.StoreAssets
                 .FirstOrDefault(a => a.Id == assetId);

            _context.Update(item);

            item.Status = _context.Statuses
                .FirstOrDefault(status => status.Name == newStatus);
        }

        private void CloseExistingCheckoutHistory(int assetId, DateTime now)
        {
            var history = _context.CheckoutHistories
              .FirstOrDefault(h => h.StoreAsset.Id == assetId
              && h.CheckedIn == null);

            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = now;
            }
        }

        private void RemoveExistingCheckouts(int assetId)
        {
            //remove any existing checkouts on the item

            var checkout = _context.Checkouts
                .FirstOrDefault(co => co.StoreAsset.Id == assetId);

            if (checkout != null)
            {
                _context.Remove(checkout);
            }
        }

        public void MarkLost(int assetId)
        {

            UpdateAssetStatus(assetId, "Lost");
            _context.SaveChanges();
        }

        public void CheckIntItem(int assetId)
        {
            var now = DateTime.Now;
            var item = _context.StoreAssets
                .FirstOrDefault(a => a.Id == assetId);

            
            // remove any existing checkouts on the item
            RemoveExistingCheckouts(assetId);

            // close any  existing checkout history
            CloseExistingCheckoutHistory(assetId, now);

            // look for existing holds on the item
            var currentHolds = _context.Holds
                .Include(h => h.StoreAsset)
                .Include(h => h.StoreCard)
                .Where(h => h.StoreAsset.Id == assetId);
            //if ther are holds, checkout the item to the storecard with the earliest hold.
            if (currentHolds.Any())
            {
                CheckOutToEarliestHold(assetId, currentHolds);
                return;
            }


            //else, update item status to availabe
            UpdateAssetStatus(assetId, "Available");

            _context.SaveChanges();
            
        }

        private void CheckOutToEarliestHold(int assetId, IQueryable<Hold> currentHolds)
        {
            var earliestHold = currentHolds
                .OrderBy(holds => holds.HoldPlaced)
                .FirstOrDefault();

            var card = earliestHold.StoreCard;
            _context.Remove(earliestHold);
            _context.SaveChanges();
            CheckOutItem(assetId, card.Id);
        }
        public void CheckOutItem(int assetId, int storeCardId)
        {
            if (IsCheckedOut(assetId)){

                return;
                //Addlogic here to handle feedback to the user
            }

            var item = _context.StoreAssets
                .FirstOrDefault(a => a.Id == assetId);

            UpdateAssetStatus(assetId, "Checked Out");

            var StoreCard = _context.StoreCards
                .Include(card => card.Checkouts)
                .FirstOrDefault(card => card.Id == storeCardId);

            var now = DateTime.Now;

            var checkout = new Checkout
            {
                StoreAsset = item,
                StoreCard = StoreCard,
                Since = now,
                Until = GetDefualtCheckoutTime(now)
            };

            _context.Add(checkout);

            var checkoutHistory = new CheckoutHistory
            {
                CheckedOut = now,
                StoreAsset = item,
                StoreCard = StoreCard
            };

            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }

        private DateTime GetDefualtCheckoutTime(DateTime now)
        {
            return now.AddDays(30);
        }
        public int GetNumberOfCopies(int assetId)
        {
            return _context.StoreAssets
                .First(a => a.Id == assetId)
                .NumberOfCopies;
        }

        public bool IsCheckedOut(int assetId)
        {
            return _context.Checkouts
                .Where(co => co.StoreAsset.Id == assetId)
                .Any();
           
        }

        public void PlaceHold(int assetId, int storeCardId)
        {
            var now = DateTime.Now;

            var asset = _context.StoreAssets
                .Include(a=>a.Status)
                .FirstOrDefault(a => a.Id == assetId);

            var card = _context.StoreCards
                .FirstOrDefault(c => c.Id == storeCardId);

            if(asset.Status.Name == "Available")
            {
                UpdateAssetStatus(assetId, "On Hold");
            }

            var hold = new Hold
            {
                HoldPlaced = now,
                StoreAsset = asset,
                StoreCard = card

            };

            _context.Add(hold);
            _context.SaveChanges();
        }


        public string GetCurrentHoldCustomerName(int HoldId)
        {
            var hold = _context.Holds
                .Include(h => h.StoreAsset)
                .Include(h => h.StoreCard)
                .FirstOrDefault(h => h.Id == HoldId);

            var cardId = hold?.StoreCard.Id;

            var customer = _context.Customers
                .Include(p => p.CustomerCard)
                .FirstOrDefault(p => p.CustomerCard.Id == cardId);

            return customer?.FirstName + " " + customer?.LastName;
        }

        public DateTime GetCurrentHoldPlaced(int HoldId)
        {
            return
                _context.Holds
                .Include(h => h.StoreAsset)
                .Include(h => h.StoreCard)
                .FirstOrDefault(h => h.Id == HoldId)
                .HoldPlaced;

        }

        public string GetCurrentCheckoutCustomer(int assetId)
        {
            var checkout = GetCheckoutByAssetId(assetId);
            if (checkout == null)
            {
              return "";
            }

            var CardId =  checkout.StoreCard.Id;

            var customer = _context.Customers
                .Include(p => p.CustomerCard)
                .First(c => c.CustomerCard.Id == CardId);


            return customer?.FirstName + " " + customer?.LastName;

        }

        private Checkout GetCheckoutByAssetId(int assetId)
        {

            return _context.Checkouts
                 .Include(co => co.StoreAsset)
                 .Include(co => co.StoreCard)
                 .FirstOrDefault(co => co.StoreCard.Id == assetId);

        }
    }
}
