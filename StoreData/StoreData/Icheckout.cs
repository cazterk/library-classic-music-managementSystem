using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreData
{
   public interface ICheckout
    {
        IEnumerable<Checkout> GetAll();
        IEnumerable<Hold> GetCurrentHolds(int id);
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);

        Checkout GetById(int checkoutId);
        Checkout GetLatestCheckout(int assetId);

        string GetCurrentCheckoutCustomer(int assetId);
        string GetCurrentHoldCustomerName(int HoldId);
        DateTime GetCurrentHoldPlaced(int id);
        bool IsCheckedOut(int id);
        int GetNumberOfCopies(int id);

        void Add(Checkout newCheckout);
        void CheckOutItem(int assetId, int storeCardId);
        void CheckIntItem(int assetId);
        void PlaceHold(int assetId, int storeCardId);
        void MarkLost(int assetId);
        void MarkFound(int assetId);

       
        

        
    }
}
