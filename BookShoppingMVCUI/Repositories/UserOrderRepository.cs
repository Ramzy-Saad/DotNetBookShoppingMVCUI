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

        public async Task ChangeOrderStatus(UpdateOrderStatusModel model)
        {
            var order = await _dbContext.Orders.FindAsync(model.orderId);
            if(order == null)
            {
                throw new Exception("Order not found.");
            }
            order.OrderStatusId = model.OrderStatusId;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _dbContext.Orders.FindAsync(orderId);
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await _dbContext.orderStatuses.ToListAsync();
        }

        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if(order ==null)
                throw new Exception("Order not found.");
            order.IsPaid = !order.IsPaid;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> UserOrders(bool getAll = false)
        {
            var orders = _dbContext.Orders
                    .Include(o => o.OrderStatus)
                    .Include(o => o.OrderDetail)
                    .ThenInclude(d => d.Book)
                    .ThenInclude(b => b.Genre).AsQueryable();
            if(!getAll)
            {
                var userId = GetUserId();
                if (userId == null)
                    throw new Exception("User is not logged in.");
                orders = orders.Where(o => o.UserId == userId);
            }
            return await orders.ToListAsync();
        }
        private string GetUserId()
        {
            var principle = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principle);
            return userId;
        }
    }
}
