using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QAAutomationPortfolio.Data;
using QAAutomationPortfolio.Extensions;
using QAAutomationPortfolio.Models;

namespace QAAutomationPortfolio.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (cartItem == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            HttpContext.Session.Set("Cart", cart);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                HttpContext.Session.Set("Cart", cart);
            }

            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        public IActionResult Checkout()
        {
            // Dummy checkout process
            HttpContext.Session.Set("Cart", new List<CartItem>());
            TempData["SuccessMessage"] = "Thank you for your order! Your purchase was successful.";
            return RedirectToAction("Index", "Home");
        }
    }
}
