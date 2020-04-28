using NHibernate;
using StoreBuy.Domain;
using StoreBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreBuy.Repositories
{
    public class StoreSeachRepository : GenericRepository<StoreInfo>, IStoreSearchRepository
    {
        public StoreSeachRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
        }

        public List<StoreInfo> FindNearestStores(long Latitude, long Longitude, List<StoreInfo> Stores)
        {
            Dictionary<StoreInfo, double> StoresMaptoDistance = new Dictionary<StoreInfo, double>();
                       
            foreach (StoreInfo Store in Stores)
            {
                var Distance = Math.Sqrt(Math.Pow(Latitude - Store.StoreLocation.Latitude, 2) + Math.Pow(Longitude - Store.StoreLocation.Longitude, 2));
                StoresMaptoDistance.Add(Store, Distance);
            }
            List<StoreInfo> NearByStores = new List<StoreInfo>();
            foreach (KeyValuePair<StoreInfo, double> Store in StoresMaptoDistance.OrderBy(key => key.Value))
            {
                NearByStores.Add(Store.Key);
                if (NearByStores.Count == 10)
                    break;
            }
            return NearByStores;
        }



        public List<ItemCatalogueModel> ItemsAvailableInStore(long UserId, StoreInfo Store)
        {
            List<ItemCatalogueModel> ItemsAvailableInStore = new List<ItemCatalogueModel>();
            try
            {
                List<Cart> CartItems = Session.Query<Cart>().Where(x => x.User.UserId == UserId).ToList();
                foreach (Cart CartItem in CartItems)
                {
                    var Item = Session.Get<ItemCatalogue>(CartItem.ItemCatalogue.ItemId);
                    var StoreItemtemp = Session.CreateSQLQuery("Select * from StoreItemCatalogue where StoreItemId = " + Item.ItemId + " and StoreId = " + Store.StoreId).AddEntity(typeof(StoreItemCatalogue));
                    var StoreItem = StoreItemtemp.List<StoreItemCatalogue>();
                    if (StoreItem.Count != 0)
                    {
                        ItemCatalogueModel ItemModel = new ItemCatalogueModel();
                        ItemModel.ItemId = Item.ItemId;
                        ItemModel.ItemDescription = Item.ItemDescription;
                        ItemModel.ItemName = Item.ItemName;
                        ItemModel.Price = StoreItem[0].StoreItemPrice;
                        ItemModel.Quantity = CartItem.Quantity;
                        ItemModel.CategoryName = Item.ItemCategory.CategoryName;
                        ItemsAvailableInStore.Add(ItemModel);
                    }
                }
                return ItemsAvailableInStore;



            }
            catch (Exception exception)
            {
                throw exception;
            }
        }



        public float IsItemAvailableInStore(long ItemId, StoreInfo Store)
        {
            var StoreItemtemp = Session.CreateSQLQuery("Select * from StoreItemCatalogue where StoreItemId = " + ItemId + " and StoreId = " + Store.StoreId).AddEntity(typeof(StoreItemCatalogue));
            var StoreItem = StoreItemtemp.List<StoreItemCatalogue>();
            if (StoreItem.Count != 0)
            {
                return StoreItem[0].StoreItemPrice;
            }
            else
            {
                return -1;
            }
        }
    }
}