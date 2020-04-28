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
    public class ItemController : ApiController
    {


        IItemRepository ItemRepository = null;
        IStoreSearchRepository StoreRepository = null;

        public ItemController(IItemRepository ItemRespository, IStoreSearchRepository StoreRepository)
        {
            this.ItemRepository = ItemRespository;
            this.StoreRepository = StoreRepository;
 
        }

        public IEnumerable<ItemCatalogue> Get()
        {

            return ItemRepository.GetAll();
        }


        [HttpGet]
        [Route("api/item/SearchRelatedItems")]
        public IEnumerable<ItemCatalogueModel> GetSearchRelatedItems(string id)
        {
            {
                List<ItemCatalogueModel> SearchedItems = new List<ItemCatalogueModel>();

                try
                {
                    var Items = ItemRepository.ItemSearch(id);
                    foreach (ItemCatalogue Item in Items)
                    {
                        ItemCatalogueModel SearchItems = new ItemCatalogueModel();
                        SearchItems.ItemId = Item.ItemId;
                        SearchItems.ItemName = Item.ItemName;
                        SearchItems.ItemDescription = Item.ItemDescription;
                        SearchItems.Price = Item.EstimatedPrice;
                        SearchItems.CategoryName = Item.ItemCategory.CategoryName;
                        SearchedItems.Add(SearchItems);
                    }
                    return SearchedItems;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        [HttpGet]
        [Route("api/item/CategoryRelatedItems")]
        public IEnumerable<ItemCatalogueModel> GetCategoryRelatedItems(long id)
        {
            List<ItemCatalogueModel> SearchedItems = new List<ItemCatalogueModel>();
            try
            {
                var Items = ItemRepository.ItemsOfCategory(id);
                foreach (ItemCatalogue Item in Items)
                {
                    ItemCatalogueModel ItemCatalogueModel = new ItemCatalogueModel();
                    ItemCatalogueModel.ItemId = Item.ItemId;
                    ItemCatalogueModel.ItemName = Item.ItemName;
                    ItemCatalogueModel.ItemDescription = Item.ItemDescription;
                    ItemCatalogueModel.Price = Item.EstimatedPrice;
                    ItemCatalogueModel.CategoryName = Item.ItemCategory.CategoryName;
                    SearchedItems.Add(ItemCatalogueModel);
                    if (SearchedItems.Count == 10)
                    {
                        break;
                    }
                }
                return SearchedItems;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("api/item/RelatedStores")]
        public Dictionary<string, float> GetRelatedStores(long ItemId)
        {
            List<StoreInfo> ListOfStores = StoreRepository.GetAll().ToList();// StoreRepository.FindNearestStores(User);
            Dictionary<string,float> StoresDictionary = new Dictionary<string, float>();

            foreach (StoreInfo Store in ListOfStores)
            {
                var price = StoreRepository.IsItemAvailableInStore(ItemId, Store);
                if(price!=-1)
                StoresDictionary.Add(Store.StoreName, price);
           
            }
            return StoresDictionary;
        }
    }
}
