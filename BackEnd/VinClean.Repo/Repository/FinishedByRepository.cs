using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IFinishedByRepository
    {
        Task<ICollection<FinshedBy>> GetFinishedByList();
        Task<ICollection<FinshedBy>> GetFinishedByListEmpID(int id);
        Task<FinshedBy> GetFinishedById(int id);
        Task<bool> AddFinishedBy(FinshedBy FinshedBy);
        Task<bool> DeleteFinishedBy(FinshedBy FinshedBy);
        Task<bool> UpdateFinishedBy(FinshedBy FinshedBy);

    }
    public class FinishedByRepository : IFinishedByRepository
    {
        private readonly ServiceAppContext _context;
        public FinishedByRepository(ServiceAppContext context)
        {
            _context = context;
        }
        async public Task<ICollection<FinshedBy>> GetFinishedByList()
        {
            return await _context.FinshedBies.ToListAsync();
            //var ListFinishedBy = _repository.GetFinishedByList();

            //var ListFinishedByDTO = ListFinishedBy.Select(finishedBy =>
            //{
            //    finishedBy.Employee = _context.Employees.FirstOrDefault(e => e.Id == finishedBy.EmployeeId);
            //    finishedBy.Order = _context.Orders.FirstOrDefault(o => o.Id == finishedBy.OrderId);
            //    return _mapper.Map<FinshedBy>(finishedBy);
            //}).ToList();

            //return async ListFinishedByDTO;
        }
        async public Task<ICollection<FinshedBy>> GetFinishedByListEmpID(int id)
        {
            return await _context.FinshedBies.Where(e=> e.EmployeeId == id).ToListAsync();
        }
        async public Task<FinshedBy> GetFinishedById(int id)
        {
            return await _context.FinshedBies.FirstOrDefaultAsync(a => a.OrderId == id);
        }

        async public Task<bool> AddFinishedBy(FinshedBy FinshedBy)
        {
            _context.FinshedBies.Add(FinshedBy);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async public Task<bool> DeleteFinishedBy(FinshedBy FinshedBy)
        {
            _context.FinshedBies.Remove(FinshedBy);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async public Task<bool> UpdateFinishedBy(FinshedBy FinshedBy)
        {
            _context.FinshedBies.Update(FinshedBy);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }


    }
}

