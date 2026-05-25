using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingMVCUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<IActionResult> AddItem(int bookId, int qty = 1, int redirect = 0)
        {
            try
            {
                var cartCount = await _cartRepository.AddItem(bookId, qty);

                if (redirect == 0)
                {
                    return Json(new
                    {
                        success = true,
                        cartCount = cartCount
                    });
                }

                return RedirectToAction("GetUserCart");
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        public async Task<IActionResult> RemoveItem(int bookId)
        {
            try
            {
                var cartCount =  await _cartRepository.RemoveItem(bookId);
                return RedirectToAction("GetUserCart");
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }
        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepository.GetCartItemCount();
            return Json(new
            {
                success = true,
                cartCount = cartItem
            });
        }
        public async Task<IActionResult> CheckOut()
        {
            bool isCheckedOut = await _cartRepository.DoCheckOut();
            if (!isCheckedOut)
                throw new Exception("Error in checkingOut.");
            return RedirectToAction("index", "Home");
        }
    }
}
