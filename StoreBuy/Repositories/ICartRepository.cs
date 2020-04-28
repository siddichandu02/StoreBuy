using StoreBuy.Domain;
using StoreBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreBuy.Repositories
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Cart RetrieveByUserandItem(Users User, ItemCatalogue itemCatalogue);
        void DeleteByUserandItem(Users User, ItemCatalogue itemCatalogue);
        IEnumerable<Cart> RetrieveCartItems(Users User);

    }
}