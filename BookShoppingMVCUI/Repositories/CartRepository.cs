using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingMVCUI.Repositories
{
    public class CartRepository: ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> AddItem(int bookId, int qty)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                string userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged in.");
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart()
                    {
                        userId = userId,
                    };
                    _dbContext.ShoppingCarts.Add(cart);
                }
                _dbContext.SaveChanges();
                var cartItem = await _dbContext.CartDetails.FirstOrDefaultAsync(c => c.ShoppingCartId == cart.Id && c.BookId == bookId);
                if (cartItem != null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    cartItem = new CartDetail()
                    {
                        BookId = bookId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty
                    };
                    _dbContext.CartDetails.Add(cartItem);
                }
                _dbContext.SaveChanges();
                transaction.Commit();
                int cartCount = await GetCartItemCount(userId);
                return cartCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> RemoveItem(int bookId)
        {
            try
            {
                string userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged in.");
                }
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    throw new Exception("Empty Cart.");
                }
                var cartItem = await _dbContext.CartDetails.FirstOrDefaultAsync(c => c.ShoppingCartId == cart.Id && c.BookId == bookId);
                if (cartItem is null)
                {
                    throw new Exception("Empty Cart Items.");
                }
                else if (cartItem.Quantity==1)
                {
                    _dbContext.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity -=1 ;
                }
                _dbContext.SaveChanges();
                int cartCount = await GetCartItemCount(userId);
                return cartCount;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null) {
                throw new Exception("Ivalid userId");
            }
            var shopingCart = await _dbContext.ShoppingCarts
                    .Include(a=>a.CartDetails)
                    .ThenInclude(a=>a.Book)
                    .ThenInclude(a=>a.Genre)
                    .Where(a=>a.userId == userId)
                    .FirstOrDefaultAsync();
            return shopingCart;
        }

        public async Task<ShoppingCart> GetCart(string userId) {
            var cart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.userId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId="")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _dbContext.ShoppingCarts
                              join CartDetail in _dbContext.CartDetails
                              on cart.Id equals CartDetail.ShoppingCartId
                              select new { CartDetail.Id }
                             ).ToListAsync();
            return data.Count;
        }
        public async Task<bool> DoCheckOut()
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged in.");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _dbContext.CartDetails
                    .Include(c => c.Book)
                    .Where(c => c.ShoppingCartId == cart.Id).ToList();
                if(cartDetail.Count == 0)
                    throw new Exception("Empty cart");

                // Creating New Order
                var order = new Order()
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = 1 // pending
                };
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail()
                    {
                        OrderId = order.Id,
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Book.Price
                    };
                    _dbContext.OrderDetails.Add(orderDetail);
                }
                _dbContext.SaveChanges();

                // Remove Cart of User
                _dbContext.CartDetails.RemoveRange(cartDetail);
                _dbContext.SaveChanges();
                transaction.Commit();
                return true;    
            }
            catch (Exception) {
                return false;
            }
        }
        private string GetUserId()
        {
            var principle = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principle);
            return userId;
        }

    }
}
