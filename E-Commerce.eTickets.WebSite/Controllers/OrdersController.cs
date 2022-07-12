using eTickets.Data.Cart;
using eTickets.Data.Services;
using eTickets.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IMoviesService _service;
        private readonly ShoppingCart _cart;
        public OrdersController(IMoviesService service, ShoppingCart cart)
        {
            _service = service;
            _cart = cart;
        }
        public IActionResult ShoppingItems()
        {
            var items = _cart.GetShoppingCartItems();
            _cart.ShoppingCartItems = items;
            var response = new ShoppingCartVM()
            {
                ShoppingCart = _cart,
                ShoppingCartTotal = _cart.GetShopingCartTotal(),
            };
            return View(response);
        }

        public async Task<RedirectToActionResult> AddItemToShoppingCart(int id)
        {
            var item = await _service.GetMovieById(id);
            if (item != null)
            {
                await _cart.AddItemToCart(item);
            }

            return RedirectToAction(nameof(ShoppingItems));
        }
        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await _service.GetMovieById(id);
            if (item != null)
            {
               await  _cart.RemoveItemFromCart(item);
            }

            return RedirectToAction(nameof(ShoppingItems));
        }
    }
}
