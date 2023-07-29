using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Models.ProcessModel;
using VinClean.Service.DTO.Order;

namespace VinClean.Repo.Repository
{
    public interface IOrderRepository
    {
        Task<ICollection<OrderModelDTO>> GetOrderList();
        Task<Order> GetOrderById(int id);
        Task<OrderModelDTO> GetInfoOrderById(int id);
        Task<bool> AddOrder(Order or);
        Task<bool> DeleteOrder(Order or);
        Task<bool> UpdateOrder(Order or);
        Task<ICollection<OrderModelDTO>> SelectOrder(SelectOrder select);
        Task<ICollection<OrderModelDTO>> SelectAllOrder(SelectOrder select);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly ServiceAppContext _context;
        public OrderRepository(ServiceAppContext context)
        {
            _context = context;
        }
        async Task<ICollection<OrderModelDTO>> IOrderRepository.GetOrderList()
        {
            /*            return await _context.Orders.ToListAsync();*/
            var result = from o in _context.Orders
                         join c in _context.Customers on o.CustomerId equals c.CustomerId
                         join ac1 in _context.Accounts on c.AccountId equals ac1.AccountId
                         join od in _context.OrderDetails on o.OrderId equals od.OrderId
                         join s in _context.Services on od.ServiceId equals s.ServiceId
                         join t in _context.Types on s.TypeId equals t.TypeId
                         join f in _context.FinshedBies on o.OrderId equals f.OrderId
                         join e in _context.Employees on f.EmployeeId equals e.EmployeeId
                         join ac2 in _context.Accounts on e.AccountId equals ac2.AccountId
                         select new OrderModelDTO
                         {
                             OrderId = o.OrderId,
                             CustomerId = (int)o.CustomerId,
                             CustomerName = ac1.Name,
                             Dob = (DateTime)ac1.Dob,
                             Address = c.Address,
                             Phone = c.Phone,
                             CustomerEmail = ac1.Email,
                             CustomerImage = ac1.Img,
                             ServiceId = (int)od.ServiceId,
                             ServiceName = s.Name,
                             TypeId = t.TypeId,
                             Type = t.Type1,
                             DateWork = o.DateWork,
                             StartTime = o.StartTime,
                             EndTime = o.EndTime,
                             StartWorking = od.StartWorking,
                             EndWorking = od.EndWorking,
                             Total = o.Total,
                             SubPrice = o.SubPrice,
                             PonitUsed = (int)o.PointUsed,
                             Status = "Completed",
                             EmployeeName = ac2.Name,
                             EmployeeImg = ac2.Img,
                             EmployeeEmail = ac2.Email,
                             EmployeePhone = e.Phone
                         };
            return await result.ToListAsync();
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

        async Task<OrderModelDTO> IOrderRepository.GetInfoOrderById(int id)
        {
            var result = from o in _context.Orders
                         join c in _context.Customers on o.CustomerId equals c.CustomerId
                         join ac1 in _context.Accounts on c.AccountId equals ac1.AccountId
                         join od in _context.OrderDetails on o.OrderId equals od.OrderId
                         join s in _context.Services on od.ServiceId equals s.ServiceId
                         join t in _context.Types on s.TypeId equals t.TypeId
                         join f in _context.FinshedBies on o.OrderId equals f.OrderId
                         join e in _context.Employees on f.EmployeeId equals e.EmployeeId
                         join ac2 in _context.Accounts on e.AccountId equals ac2.AccountId
                         where o.OrderId == id
                         select new OrderModelDTO
                         {
                             OrderId = o.OrderId,
                             CustomerId = (int)o.CustomerId,
                             CustomerName = ac1.Name,
                             Dob = (DateTime)ac1.Dob,
                             Address = c.Address,
                             Phone = c.Phone,
                             CustomerEmail = ac1.Email,
                             CustomerImage = ac1.Img,
                             ServiceId = (int)od.ServiceId,
                             ServiceName = s.Name,
                             TypeId = t.TypeId,
                             Type = t.Type1,
                             DateWork = o.DateWork,
                             StartTime = o.StartTime,
                             EndTime = o.EndTime,
                             StartWorking = od.StartWorking,
                             EndWorking = od.EndWorking,
                             Total = o.Total,
                             SubPrice = o.SubPrice,
                             PonitUsed = (int)o.PointUsed,
                             Status = "Completed",
                             EmployeeName = ac2.Name,
                             EmployeeImg = ac2.Img,
                             EmployeeEmail = ac2.Email,
                             EmployeePhone = e.Phone
                         };
            return await result.FirstOrDefaultAsync();
        }


        async Task<bool> IOrderRepository.UpdateOrder(Order or)
        {
            _context.Orders.Update(or);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<ICollection<OrderModelDTO>> SelectOrder(SelectOrder select)
        {
            int employeeId = select.EmployeeId;
            DateTime startDate = DateTime.Parse(select.StartMonth);
            DateTime endDate = DateTime.Parse(select.EndMonth);

            var query = (from o in _context.Orders
                        join fb in _context.FinshedBies on o.OrderId equals fb.OrderId
                         join c in _context.Customers on o.CustomerId equals c.CustomerId
                         join e in _context.Employees on fb.EmployeeId equals e.EmployeeId
                        join od in _context.OrderDetails on o.OrderId equals od.OrderId
                        join s in _context.Services on od.ServiceId equals s.ServiceId
                         join t in _context.Types on s.TypeId equals t.TypeId
                         where (o.DateWork >= startDate && o.DateWork <= endDate) && fb.EmployeeId == employeeId
                        select new OrderModelDTO
                        {
                            Address = c.Address,
                            CustomerName = c.FirstName + c.LastName,
                            Type = t.Type1,
                            SubPrice = o.SubPrice,
                            Phone = c.Phone,
                            PonitUsed =(int)o.PointUsed,
                            StartTime = o.StartTime,
                            EndTime = o.EndTime,
                            OrderId = o.OrderId,
                            DateWork = o.DateWork,
                            StartWorking = od.StartWorking,
                            EndWorking = od.EndWorking,
                            Total = o.Total,
                            ServiceName=s.Name

                        });
        return await query.ToListAsync();
        }

        public async Task<ICollection<OrderModelDTO>> SelectAllOrder(SelectOrder select)
        {
            int employeeId = select.EmployeeId;
            DateTime startDate = DateTime.Parse(select.StartMonth);
            DateTime endDate = DateTime.Parse(select.EndMonth);

            var query = (from o in _context.Orders
                         join fb in _context.FinshedBies on o.OrderId equals fb.OrderId
                         join c in _context.Customers on o.CustomerId equals c.CustomerId
                         join e in _context.Employees on fb.EmployeeId equals e.EmployeeId
                         join od in _context.OrderDetails on o.OrderId equals od.OrderId
                         join s in _context.Services on od.ServiceId equals s.ServiceId
                         join t in _context.Types on s.TypeId equals t.TypeId
                         join ac2 in _context.Accounts on e.AccountId equals ac2.AccountId
                         where (o.DateWork >= startDate && o.DateWork <= endDate) 
                         select new OrderModelDTO
                         {
                             Address = c.Address,
                             CustomerName = c.FirstName + c.LastName,
                             Type = t.Type1,
                             SubPrice = o.SubPrice,
                             Phone = c.Phone,
                             PonitUsed = (int)o.PointUsed,
                             StartTime = o.StartTime,
                             EndTime = o.EndTime,
                             OrderId = o.OrderId,
                             DateWork = o.DateWork,
                             StartWorking = od.StartWorking,
                             EndWorking = od.EndWorking,
                             Total = o.Total,
                             ServiceName = s.Name,
                             EmployeeName = e.FirstName + e.LastName,
                             EmployeeId = e.EmployeeId,
                             EmployeeImg = ac2.Img

                         });
            return await query.ToListAsync();
        }
    }
    }

