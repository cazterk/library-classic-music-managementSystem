
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoreData.Models
{
    public class Customer
    {
        [StringLength(30)]
        public int Id { get; set; }

        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string LastName { get; set; }

        [StringLength(30)]
        public string Address { get; set; }


        public DateTime DateOfBirth { get; set; }
        public string TelephoneNumber { get; set; }

       public virtual StoreCard CustomerCard { get; set; }
       public virtual StoreBranch HomeStoreBranch { get; set; }

    }
}
