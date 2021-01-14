using StoreData.Models;
using System;
using System.Collections.Generic;

namespace StoreManagementSystem.Models.CustomerModel
{
    public class CustomerDetailModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int StoreCardId { get; set; }
        public string Address { get; set; }
        public DateTime MemberSince { get; set; }
        public string Telephone { get; set; }
        public string HomeStoreBranch { get; set; }
        public decimal Overdue { get; set; }
        public IEnumerable<Checkout> AssetsCheckedOut { get; set; }
        public IEnumerable<CheckoutHistory> CheckOutHistory { get; set; }
        public IEnumerable<Hold> Holds { get; set; }

        public string FullName
        {
            get{return FirstName + " " + LastName;}
        }
        

    }
}
