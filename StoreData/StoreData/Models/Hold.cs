using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreData.Models
{
    public class Hold
    {

        public int Id { get; set; }        
        public virtual StoreAsset StoreAsset { get; set; }
        public virtual StoreCard StoreCard { get; set; }
        public DateTime HoldPlaced { get; set; }
    }
}
