using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagementSystem.Models.CustomerModel
{
    public class CustomerIndexModel
    {

        public IEnumerable<CustomerDetailModel> Customers { get; set; }
    }
}
