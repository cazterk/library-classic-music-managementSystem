using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreData
{
   public interface IStoreAsset
    {
        IEnumerable<StoreAsset> GetAll();
        StoreAsset GetById(int id);


        void Add(StoreAsset newAsset);
        string GetAuthorOrDirector(int id);
        string GetDeweyIndex(int id);
        string GetType(int id);
        string GetTitle(int id);
        string GetIsbn(int id);

        StoreBranch GetCurrentLocation(int id);
    }
}
