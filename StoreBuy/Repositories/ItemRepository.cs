using NHibernate;
using StoreBuy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreBuy.Repositories
{
    public class ItemRepository: GenericRepository<ItemCatalogue>,IItemRepository
    {        

       public ItemRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {          
        }       

        public IEnumerable<ItemCatalogue> ItemSearch(string SearchString)
        {
            
            try
            {
                var SearchItems = Session.CreateSQLQuery("Select * from ItemCatalogue where ItemName like '%" + SearchString + "%'").AddEntity(typeof(ItemCatalogue));
                return  SearchItems.List<ItemCatalogue>();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<ItemCatalogue> ItemsOfCategory(long id)
        {
            try
            {
                var CategoryItems = Session.CreateSQLQuery("Select * from ItemCatalogue where ItemCategoryId = " + id).AddEntity(typeof(ItemCatalogue));
                return CategoryItems.List<ItemCatalogue>();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}