using E_Commerce.eTickets.WebSite.data;
using E_Commerce.eTickets.WebSite.Models;
using eTickets.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Cart
{
    public class ShoppingCart
    {
        public AppDbContext _context { get; set; }
        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public ShoppingCart(AppDbContext context)
        {
            _context = context;
        }
        //conf sessions as a service
        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<AppDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public async Task AddItemToCart(Movie movie)
        {
            var item = _context.ShoppingCartItems.FirstOrDefault(x => x.Movie.Id == movie.Id && x.ShopingCartId == ShoppingCartId);

            if (item != null)
            {
                item.Amount++;
                await _context.SaveChangesAsync();
                return;
            }

            item = new ShoppingCartItem()
            {
                ShopingCartId = ShoppingCartId,
                Movie = movie,
                Amount = 1
            };
            await _context.ShoppingCartItems.AddAsync(item);

            await _context.SaveChangesAsync();

        }
        public async Task RemoveItemFromCart(Movie movie)
        {
            var item = _context.ShoppingCartItems.FirstOrDefault(x => x.Movie.Id == movie.Id && x.ShopingCartId == ShoppingCartId);
            if (item == null) return;

            if (item.Amount > 1)
                item.Amount--;
            else
                _context.ShoppingCartItems.Remove(item);

            await _context.SaveChangesAsync();
        }
        public List<ShoppingCartItem> GetShoppingCartItems() => ShoppingCartItems ??
            (ShoppingCartItems = _context.ShoppingCartItems
            .Where(x => x.ShopingCartId == ShoppingCartId)
                .Include(x => x.Movie)
                .ToList());
        public double GetShopingCartTotal() => _context.ShoppingCartItems
            .Where(x => x.ShopingCartId == ShoppingCartId)
                .Select(x => x.Movie.Price * x.Amount).Sum();
    }
}
