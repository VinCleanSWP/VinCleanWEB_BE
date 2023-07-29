using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IOrderDetailRepository
    {
        Task<ICollection<OrderDetail>> GetOrderDetailList();
        Task<OrderDetail> GetOrderDetailById(int id);
        Task<bool> AddOrderDetail(OrderDetail od);
        Task<bool> DeleteOrderDetail(OrderDetail od);
        Task<bool> UpdateOrderDetail(OrderDetail od);
    }
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ServiceAppContext _context;
        public OrderDetailRepository(ServiceAppContext context)
        {
            _context = context;
        }
        async Task<bool> IOrderDetailRepository.AddOrderDetail(OrderDetail od)
        {
            _context.OrderDetails.Add(od);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IOrderDetailRepository.DeleteOrderDetail(OrderDetail od)
        {
            _context.OrderDetails.Remove(od);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<OrderDetail> IOrderDetailRepository.GetOrderDetailById(int id)
        {
            return await _context.OrderDetails.FirstOrDefaultAsync(a => a.OrderId == id);
        }

        async Task<ICollection<OrderDetail>> IOrderDetailRepository.GetOrderDetailList()
        {
            return await _context.OrderDetails.ToListAsync();
        }

        async Task<bool> IOrderDetailRepository.UpdateOrderDetail(OrderDetail od)
        {
            _context.OrderDetails.Update(od);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
