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
        Task<IEnumerable<WorkingBy>> GetWorkingByListByEpmId(int id);
        Task<WorkingBy> GetWorkingByById(int id);
        Task<bool> Check(int id);
        Task<WorkingBy> GetWorkingByByProcessId(int id);
        Task<bool> UpdateWorkingBy(WorkingBy customer);
        Task<bool> AddWorkingBy(WorkingBy slot);
        Task<bool> DeleteWorkingBy(WorkingBy workingBy);
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
        async Task<IEnumerable<WorkingBy>> IWorkingByRepository.GetWorkingByListByEpmId(int id)
        {
            return await _context.WorkingBies.Where(e=>e.EmployeeId == id).ToListAsync();
        }
        async Task<WorkingBy> IWorkingByRepository.GetWorkingByById(int id)
        {
            return await _context.WorkingBies.FirstOrDefaultAsync(a => a.EmployeeId == id);
        }
         async Task<bool> IWorkingByRepository.Check(int id)
        {
            return await _context.WorkingBies.AnyAsync(a => a.EmployeeId == id);
        }
        async Task<WorkingBy> IWorkingByRepository.GetWorkingByByProcessId(int id)
        {
            return await _context.WorkingBies.FirstOrDefaultAsync(a => a.ProcessId == id);
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

        async Task<bool> IWorkingByRepository.DeleteWorkingBy(WorkingBy workingBy)
        {
             
                _context.WorkingBies.Remove(workingBy);
                return await _context.SaveChangesAsync() > 0 ? true : false; ;


        }

    }
}
