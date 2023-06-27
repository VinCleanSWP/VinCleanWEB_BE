using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IWorkingByRepository
    {
        Task<ICollection<WorkingBy>> GetWorkingByList();
        Task<WorkingBy> GetWorkingByById(int id);
        Task<bool> UpdateWorkingBy(WorkingBy customer);
        Task<bool> AddWorkingBy(WorkingBy slot);
        Task<bool> DeleteWorkingBy(int id);
    }
    public class WorkingByRepository : IWorkingByRepository
    {
        private readonly ServiceAppContext _context;
        public WorkingByRepository(ServiceAppContext context)
        {
           _context = context;
        }
        async Task<ICollection<WorkingBy>> IWorkingByRepository.GetWorkingByList()
        {
            return await _context.WorkingBies.ToListAsync();
        }
        async Task<WorkingBy> IWorkingByRepository.GetWorkingByById(int id)
        {
            return await _context.WorkingBies.FirstOrDefaultAsync(a => a.EmployeeId == id);
        }

        async Task<bool> IWorkingByRepository.UpdateWorkingBy(WorkingBy slot)
        {
            _context.WorkingBies.Update(slot);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IWorkingByRepository.AddWorkingBy(WorkingBy slot)
        {
            _context.WorkingBies.Add(slot);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IWorkingByRepository.DeleteWorkingBy(int id)
        {
            var _exisitngWB = await _context.WorkingBies.FirstOrDefaultAsync(a => a.EmployeeId == id);
            if (_exisitngWB != null)
            {
                _context.WorkingBies.Remove(_exisitngWB);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
