using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IServiceManageRepository
    {
        Task<List<ServiceManage>> GetServiceManages();
        Task<ServiceManage> GetServiceManageById(int employeeId, int serviceId);
        Task<bool> AddServiceManage(ServiceManage serviceManage);
        Task<bool> DeleteServiceManage(ServiceManage serviceManage);
        Task<bool> UpdateServiceManage(ServiceManage serviceManage);
    }

    public class ServiceManageRepository : IServiceManageRepository
    {
        private readonly ServiceAppContext _context;

        public ServiceManageRepository(ServiceAppContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceManage>> GetServiceManages()
        {
            return await _context.ServiceManages.ToListAsync();
        }

        public async Task<ServiceManage> GetServiceManageById(int employeeId, int serviceId)
        {
            return await _context.ServiceManages
                .FirstOrDefaultAsync(sm => sm.EmployeeId == employeeId && sm.ServiceId == serviceId);
        }

        public async Task<bool> AddServiceManage(ServiceManage serviceManage)
        {
            _context.ServiceManages.Add(serviceManage);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteServiceManage(ServiceManage serviceManage)
        {
            _context.ServiceManages.Remove(serviceManage);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateServiceManage(ServiceManage serviceManage)
        {
            _context.ServiceManages.Update(serviceManage);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
