using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingMVCUI.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;

        public UserOrderRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("User is not logged in.");
            var orders = await _dbContext.Orders
                    .Include(o=>o.OrderStatus)
                    .Include(o=>o.OrderDetail)
                    .ThenInclude(d=>d.Book)
                    .ThenInclude(b=>b.Genre)
                    .Where(o => o.UserId == userId)
                    .ToListAsync();
            return orders;
        }

        private string GetUserId()
        {
            var principle = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principle);
            return userId;
        }
    }
}
