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
    public interface IProcessSlotRepository
    {
        Task<ICollection<ProcessSlot>> GetPSList();
        Task<ProcessSlot> GetPSById(int id);
        Task<bool> AddPS(ProcessSlot processSlot);
        Task<bool> UpdatePS(ProcessSlot processSlot);
        Task<bool> DeletePS(ProcessSlot processSlot);
    }
    public class ProcessSlotRepository : IProcessSlotRepository
    {
        private readonly ServiceAppContext _context;

        public ProcessSlotRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<ICollection<ProcessSlot>> IProcessSlotRepository.GetPSList()
        {
            return await _context.ProcessSlots.Include(e => e.Process).Include(e => e.Slot).ToListAsync();
        }

        async Task<ProcessSlot> IProcessSlotRepository.GetPSById(int id)
        {
            return await _context.ProcessSlots.Include(e => e.Process).Include(e => e.Slot).FirstOrDefaultAsync(ps => ps.ProcessId == id);
        }

        async Task<bool> IProcessSlotRepository.AddPS(ProcessSlot processSlot)
        {
            _context.Add(processSlot);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IProcessSlotRepository.UpdatePS(ProcessSlot processSlot)
        {
            _context.Update(processSlot);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IProcessSlotRepository.DeletePS(ProcessSlot processSlot)
        {
            _context.Remove(processSlot);
            return await _context.SaveChangesAsync() > 0 ? true: false;
        }
    }
}
