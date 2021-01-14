using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreData.Models
{
    public class CheckoutHistory
    {
        public int Id { get; set; }


        [Required]
        public StoreAsset StoreAsset { get; set; }

        [Required]
        public StoreCard StoreCard { get; set; }

        [Required]
        public DateTime CheckedOut { get; set; }

        public DateTime? CheckedIn { get; set; }
    }
}
