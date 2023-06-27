using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

// lay table

namespace VinClean.Repo.Repository
{
    public interface IProcessDetailRepository
    {
        Task<ICollection<ProcessDetail>> GetPDList();

        Task<ProcessDetail> GetPDById(int id);
        Task<bool> AddPD(ProcessDetail processDetail);
        Task<bool> UpdatePD(ProcessDetail processDetail);
        Task<bool> DeletePD(ProcessDetail processDetail);
    }
    public class ProcessDetailRepository : IProcessDetailRepository
    {
        private readonly ServiceAppContext _context;
        public ProcessDetailRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<ICollection<ProcessDetail>> IProcessDetailRepository.GetPDList()
        {
            return await _context.ProcessDetails.Include(e=>e.Process).Include(e=>e.Service).ToListAsync();
        }

        async Task<ProcessDetail> IProcessDetailRepository.GetPDById(int id)
        {
            return await _context.ProcessDetails.Include(e => e.Process).Include(e => e.Service).FirstOrDefaultAsync(pd => pd.ProcessId == id);
        }

        async Task<bool> IProcessDetailRepository.AddPD(ProcessDetail processDetail)
        {
            _context.Entry(processDetail).State = EntityState.Added;
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IProcessDetailRepository.DeletePD(ProcessDetail processDetail)
        {
            _context.Update(processDetail);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IProcessDetailRepository.UpdatePD(ProcessDetail processDetail)
        {
            _context.Remove(processDetail);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
