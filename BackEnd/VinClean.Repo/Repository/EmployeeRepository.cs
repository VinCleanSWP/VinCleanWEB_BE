using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IEmployeeRepository
    {
        Task<ICollection<Employee>> GetEmployeeList();
        Task<Employee> GetEmployeeById(int id);
        Task<bool> AddEmployee(Employee employee);
        Task<bool> DeleteEmployee(Employee employee);
        Task<bool> UpdateEmployee(Employee employee);
        
    }
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ServiceAppContext _context;
        public EmployeeRepository(ServiceAppContext context)
        {
            _context = context;
        }
        async public Task<ICollection<Employee>> GetEmployeeList()
        {
            return await _context.Employees.Include(e=>e.Account).ToListAsync();
        }

        async public Task<Employee> GetEmployeeById(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(a => a.EmployeeId == id);
        }

        async public Task<bool> AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async public Task<bool> DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async public Task<bool> UpdateEmployee(Employee employee)
        {
            _context.Employees.Update(employee);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        
    }
}
