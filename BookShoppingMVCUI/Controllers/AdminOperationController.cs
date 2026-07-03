using BookShoppingMVCUI.Constants;
using BookShoppingMVCUI.Interfaces;
using BookShoppingMVCUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShoppingMVCUI.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class AdminOperationController : Controller
    {
        private readonly IUserOrderRepository _userOrderRepository;

        public AdminOperationController(IUserOrderRepository userOrderRepository )
        {
            _userOrderRepository = userOrderRepository;
        }
        public async Task<IActionResult> AllOrders()
        {
            var orders = await _userOrderRepository.UserOrders(true);
            return View(orders);
        }

        public async Task<IActionResult> TogglePaymentStatus(int orderId)
        {
            try
            {
                await _userOrderRepository.TogglePaymentStatus(orderId);
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction(nameof(AllOrders));
        }

        public async Task<IActionResult> UpdateOrderStatus(int orderId)
        {
            var order = await _userOrderRepository.GetOrderById(orderId);
            if (order == null)
                throw new InvalidOperationException($"Order with id: {orderId} does not found.");

            var orderStatusList = (await _userOrderRepository.GetOrderStatuses()).Select(
                orderStatus =>
                {
                    return new SelectListItem
                    {
                        Value = orderStatus.Id.ToString(),
                        Text = orderStatus.Name,
                        Selected = order.OrderStatusId == orderStatus.Id
                    };
                }
            );
            var data = new UpdateOrderStatusModel
            {
                OrderId = orderId,
                OrderStatusId = order.OrderStatusId,
                OrderStatusList = orderStatusList
            };
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatusModel data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                   data.OrderStatusList  = (await _userOrderRepository.GetOrderStatuses()).Select(
                       orderStatus =>
                       {
                           return new SelectListItem
                           {
                               Value = orderStatus.Id.ToString(),
                               Text = orderStatus.Name,
                               Selected = data.OrderStatusId == orderStatus.Id
                           };
                       }
                   );
                    return View(data);
                }
                await _userOrderRepository.ChangeOrderStatus(data);
                TempData["msg"] = "Order status updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["msg"] = $"An error occurred while updating order status: {ex.Message}";
            }   
            return RedirectToAction(nameof(UpdateOrderStatus),new {orderId = data.OrderId });
        }

        public IActionResult Dashboard()
        {
            return View();
        }

    }
}
