using kurs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace kurs.Services
{
    public class OrderManager
    {
        private readonly DatabaseContext _context;

        public OrderManager(DatabaseContext context)
        {
            _context = context;
        }
        public List<Order> GetEntities()
        {
            return _context.Orders.ToList();
        }
        public Order? GetEntity(int? id)
        {
            return _context.Orders.Find(id);
        }
        public async Task<EntityEntry<Order>> Add(Order order)
        {
            var result = await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return result;
        }
        public async Task AddProducts(Order order, List<CartItem> items)
        {
            foreach (var item in items)
            {
                await _context.OrderProducts.AddAsync(new OrderProduct
                {
                    Amount = item.Amount,
                    OrderId = order.Id,
                    ProductId = item.Product.Id
                });
            }
            await _context.SaveChangesAsync();
        }
        public async Task<EntityEntry<Order>> Delete(Order order)
        {
            var result = _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return result;
        }

    }
}
