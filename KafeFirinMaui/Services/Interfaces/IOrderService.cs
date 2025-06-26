using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeFirinMaui.Services.Interfaces
{
    public interface IOrderService
    {
        Task<bool> PlaceOrder(OrderRequest request);
        Task<List<Orders>> GetOrdersAsync();
        Task<List<Orders>> GetOrdersByStaffIdAsync(int staffId);
        Task<bool> UpdateOrderStatusAsync(Orders orders);
        Task<int> GetUserOrderCountAsync(int userId);
    }
}
