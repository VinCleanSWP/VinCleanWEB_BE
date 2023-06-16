using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IServiceRepository
    {
        Task<ICollection<Service>> GetServiceList();
        Task<Service> GetServiceById(int id);
        Task<bool> AddService(Service service);
        Task<bool> DeleteService(Service service);
        Task<bool> UpdateService(Service service);
    }

    public class ServiceRepository : IServiceRepository
    {
        private readonly ServiceAppContext _context;

        public ServiceRepository(ServiceAppContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Service>> GetServiceList()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task<Service> GetServiceById(int id)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == id);
        }

        public async Task<bool> AddService(Service service)
        {
            _context.Services.Add(service);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteService(Service service)
        {
            _context.Services.Remove(service);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateService(Service service)
        {
            _context.Services.Update(service);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
