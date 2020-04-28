using StoreBuy.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreBuy.Repositories
{
    public interface IItemRepository: IGenericRepository<ItemCatalogue>
    {
        IEnumerable<ItemCatalogue> ItemSearch(string SearchString);
        IEnumerable<ItemCatalogue> ItemsOfCategory(long id);



    }
}