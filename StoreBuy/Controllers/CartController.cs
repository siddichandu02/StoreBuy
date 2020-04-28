using StoreBuy.Repositories;
using StoreBuy.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreBuy.Models;

namespace StoreBuy.Controllers
{
    public class CartController : ApiController
    {


        ICartRepository CartRepository = null;
        IGenericRepository<Users> UserRepository = null;
        IItemRepository ItemRepository = null;
        
        public CartController(ICartRepository CartRepository,IGenericRepository<Users> UserRepository,IItemRepository ItemRepository)
        {
            this.CartRepository = CartRepository;
            this.UserRepository = UserRepository;
            this.ItemRepository = ItemRepository;
            
        }
        
        [HttpPost]
        [Route("api/cart/AddToCart")]
        public void PostAddToCart(long UserId,long ItemId)
        {
            Users User = UserRepository.GetById(UserId);
            ItemCatalogue ItemCatalogue = ItemRepository.GetById(ItemId);

            Cart Cart =CartRepository.RetrieveByUserandItem(User, ItemCatalogue);
            if(Cart==null)
            {
                Cart = new Cart();
                Cart.ItemCatalogue = ItemCatalogue;
                Cart.User = User;
                Cart.Quantity = 1;
            }
            else
            {
                Cart.Quantity += 1;

            }
            CartRepository.InsertOrUpdate(Cart);
        }

        //update quantity of item from the box in cart
        [HttpPost]
        [Route("api/cart/UpdateQuantity")]
        public void PostUpdateQuantity(long UserId, long ItemId, int Quantity)
        {
            Users User = UserRepository.GetById(UserId);
            ItemCatalogue ItemCatalogue = ItemRepository.GetById(ItemId);

            Cart Cart = CartRepository.RetrieveByUserandItem(User, ItemCatalogue);
            Cart.Quantity = Quantity;
            CartRepository.InsertOrUpdate(Cart);
        }

        public void DeleteItem(long UserId, long ItemId)
        {
            Users User = UserRepository.GetById(UserId);
            ItemCatalogue ItemCatalogue = ItemRepository.GetById(ItemId);

            CartRepository.DeleteByUserandItem(User,ItemCatalogue);
        }
        [HttpGet]
        [Route("api/cart/CartItems")]
        public IEnumerable<CartItemModel> GetCartItems(long UserId)
        {
       
            List<CartItemModel> CartList = new List<CartItemModel>();
            Users User = UserRepository.GetById(UserId);
            var CartItems = CartRepository.RetrieveCartItems(User);
            foreach (Cart Cart in CartItems)
            {
                CartItemModel CartItem = new CartItemModel();
                CartItem.CartId = Cart.CartId;
                CartItem.ItemId = Cart.ItemCatalogue.ItemId;
                CartItem.Quantity = Cart.Quantity;
                CartItem.UserId = Cart.User.UserId;
                CartList.Add(CartItem);
                
            }
            return CartList;
        }
      
    }
}