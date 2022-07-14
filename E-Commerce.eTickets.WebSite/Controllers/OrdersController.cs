using eTickets.Data.Cart;
using eTickets.Data.Services;
using eTickets.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IMoviesService _service;
        private readonly ShoppingCart _cart;
        private readonly IOrdersService _ordersService;
        public OrdersController(IMoviesService service, ShoppingCart cart, IOrdersService ordersService)
        {
            _service = service;
            _cart = cart;
            _ordersService = ordersService;
        }
        public async Task<IActionResult> Index() {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View(orders);
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

        public async Task<IActionResult> CompleteOrder() {
            var items = _cart.GetShoppingCartItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAdress = User.FindFirstValue(ClaimTypes.Email);
            await _ordersService.StoreOrder(items, userId, userEmailAdress);
            await _cart.ClearShoppingCart();
            return View();
        }
    }
}
