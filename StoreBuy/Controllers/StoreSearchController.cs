using Newtonsoft.Json;
using StoreBuy.Domain;
using StoreBuy.Models;
using StoreBuy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StoreBuy.Controllers
{
    public class StoreSearchController : ApiController
    {

        ICartRepository CartRepository = null;IStoreSearchRepository StoreRepository = null;

       public  StoreSearchController(ICartRepository CartRepository,IStoreSearchRepository StoreRepository)
        {
            this.CartRepository = CartRepository;
            this.StoreRepository = StoreRepository;
            
        }
       
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        
        public string Get(int id)
        {
            return "value";
        }
        [HttpGet]
        [Route("api/StoreSearch/FindStores")]
        public List<StoreModel> GetFindStores(long UserId, long Latitude,long Longitude)
        {

            List<StoreInfo> AllListOfStores = StoreRepository.GetAll().ToList();
            List<StoreInfo> ListOfStores =  StoreRepository.FindNearestStores(Latitude,Longitude,AllListOfStores);
            Dictionary<StoreInfo, List<ItemCatalogueModel>> StoresDictionary = new Dictionary<StoreInfo, List<ItemCatalogueModel>>();

            foreach (StoreInfo Store in ListOfStores)
            {
                List<ItemCatalogueModel> ItemsList = StoreRepository.ItemsAvailableInStore(UserId, Store);
                StoresDictionary.Add(Store, ItemsList);
            }
            List<StoreModel> Stores = new List<StoreModel>();
            foreach (KeyValuePair<StoreInfo, List<ItemCatalogueModel>> Item in StoresDictionary.OrderBy(key => -key.Value.Count))
            {
                var Storemodel = new StoreModel();
                Storemodel.StoreId = Item.Key.StoreId;
                Storemodel.StoreName = Item.Key.StoreName;
                Storemodel.Phone = Item.Key.Phone;
                Storemodel.ListItems = Item.Value;
                Stores.Add(Storemodel);
            }
            return Stores;
        }

    }
}
