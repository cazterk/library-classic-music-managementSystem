using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreData.Models
{
    public class Checkout
    {
        public int Id { get; set; }

        [Required]
        public StoreAsset StoreAsset { get; set; }
        public StoreCard StoreCard { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
    }
}
 