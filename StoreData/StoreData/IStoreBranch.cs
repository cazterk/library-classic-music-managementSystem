using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreData
{
   public interface IStoreBranch
    {
        IEnumerable<StoreBranch> Getall();
        IEnumerable<Customer> GetCustomers(int branchId);
        IEnumerable<StoreAsset> GetAssets(int branchId);
        IEnumerable<string> GetBranchHours(int branchId);
        StoreBranch Get(int branchId);
        void Add(StoreBranch newBranch);
        bool IsBranchOpen(int branchId);
    }
}
