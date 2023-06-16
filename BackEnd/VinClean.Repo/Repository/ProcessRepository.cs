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
    public interface IProcessRepository
    {
        Task<ICollection<Process>> GetProcesslist();
        Task<Process> GetProcessById(int id);
        Task<bool> AddProcess(Process process);
        Task<bool> UpdateProcess(Process process);
        Task<bool> DeleteProcess(Process process);

    }
    public class ProcessRepository : IProcessRepository
    {
        private readonly ServiceAppContext _context;
        public ProcessRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<ICollection<Process>> IProcessRepository.GetProcesslist()
        {
            return await _context.Processes.ToListAsync();
        }
        async Task<Process> IProcessRepository.GetProcessById(int id)
        {
            return await _context.Processes.FirstOrDefaultAsync(p => p.ProcessId == id);
        }

        async Task<bool> IProcessRepository.AddProcess(Process process)
        {
            _context.Processes.Add(process);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IProcessRepository.UpdateProcess(Process process)
        {
            _context.Processes.Update(process);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IProcessRepository.DeleteProcess(Process process)
        {
            _context.Processes.Remove(process);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
