using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IOrderRepository
    {
        Task<ICollection<Order>> GetOrderList();
        Task<Order> GetOrderById(int id);
        Task<bool> AddOrder(Order or);
        Task<bool> DeleteOrder(Order or);
        Task<bool> UpdateOrder(Order or);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly ServiceAppContext _context;
        public OrderRepository(ServiceAppContext context)
        {
            _context = context;
        }
        async Task<ICollection<Order>> IOrderRepository.GetOrderList()
        {
            return await _context.Orders.ToListAsync();
        }
        async Task<bool> IOrderRepository.AddOrder(Order or)
        {
            _context.Orders.Add(or);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IOrderRepository.DeleteOrder(Order or)
        {
            _context.Orders.Remove(or);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<Order> IOrderRepository.GetOrderById(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(a => a.OrderId == id);
        }


        async Task<bool> IOrderRepository.UpdateOrder(Order or)
        {
            _context.Orders.Update(or);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
