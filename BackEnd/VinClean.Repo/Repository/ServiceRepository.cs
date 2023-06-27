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
        Task<ICollection<Service>> GetServiceListById(int id);
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

        async Task<bool> IServiceRepository.AddService(Service service)
        {
            _context.Services.Add(service);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IServiceRepository.DeleteService(Service service)
        {
            _context.Services.Remove(service);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<Service> IServiceRepository.GetServiceById(int id)
        {
            return await _context.Services.FirstOrDefaultAsync(a => a.ServiceId == id);
        }



        async Task<ICollection<Service>> IServiceRepository.GetServiceList()
        {
            return await _context.Services.ToListAsync();
        }

        async Task<ICollection<Service>> IServiceRepository.GetServiceListById(int id)
        {
            return await _context.Services.Where(e => e.TypeId == id).ToListAsync();
        }



        async Task<bool> IServiceRepository.UpdateService(Service service)
        {
            _context.Services.Update(service);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
