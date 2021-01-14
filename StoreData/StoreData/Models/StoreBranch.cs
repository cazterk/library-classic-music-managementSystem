using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreData.Models
{
    public class StoreBranch
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Telephone { get; set; }
        public string Description { get; set; }
        public DateTime OpenDate { get; set; }

        public virtual IEnumerable<Customer> Customers { get; set; }
        public virtual IEnumerable<StoreAsset> StoreAssets { get; set; }

        public string ImageUrl { get; set; }
    }
}
