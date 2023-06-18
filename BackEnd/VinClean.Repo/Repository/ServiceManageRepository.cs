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
        Task<ICollection<ServiceManage>> GetServiceManageList();
        Task<ServiceManage> GetServiceManageById(int ServiceId);
        Task<bool> CreateServiceManage(ServiceManage serviceManage);
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

        async Task<bool> IServiceManageRepository.CreateServiceManage(ServiceManage serviceManage)
        {
            _context.ServiceManages.Add(serviceManage);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IServiceManageRepository.DeleteServiceManage(ServiceManage serviceManage)
        {
            _context.ServiceManages.Remove(serviceManage);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<ServiceManage> IServiceManageRepository.GetServiceManageById(int ServiceId)
        {
            return await _context.ServiceManages.FirstOrDefaultAsync(a => a.ServiceId == ServiceId);
        }



        async Task<ICollection<ServiceManage>> IServiceManageRepository.GetServiceManageList()
        {
            return await _context.ServiceManages.ToListAsync();
        }



        async Task<bool> IServiceManageRepository.UpdateServiceManage(ServiceManage serviceManage)
        {
            _context.ServiceManages.Update(serviceManage);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
