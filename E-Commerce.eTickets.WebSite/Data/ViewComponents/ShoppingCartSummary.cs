using eTickets.Data.Cart;
using Microsoft.AspNetCore.Mvc;

namespace eTickets.Data.ViewComponents
{
    public class ShoppingCartSummary : ViewComponent
    {
        public readonly ShoppingCart _cart;
        public ShoppingCartSummary(ShoppingCart cart)
        {
            _cart = cart;
        }
        public IViewComponentResult Invoke() 
        {
            var items = _cart.GetShoppingCartItems();
            return View(items.Count);
        }
    }
}
