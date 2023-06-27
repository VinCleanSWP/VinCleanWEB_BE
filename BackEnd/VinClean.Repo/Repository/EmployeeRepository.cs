using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Models.ProcessModel;

namespace VinClean.Repo.Repository
{
    public interface IEmployeeRepository
    {
        Task<ICollection<Employee>> GetEmployeeList();
        Task<Employee> GetEmployeeById(int id);

        Task<ICollection<Employee>> SelectEmployeeList(String startTime, String endTime, String date);
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

        public async Task<ICollection<Employee>> SelectEmployeeList(String startTime, String endTime, String date)
        {
            TimeSpan startTimeSpan = TimeSpan.Parse(startTime);
            TimeSpan endTimeSpan = TimeSpan.Parse(endTime);
            DateTime dateValue = DateTime.Parse(date);

            var query = from e in _context.Employees
                        join wb in _context.WorkingBies on e.EmployeeId equals wb.EmployeeId into workingByJoin
                        from wb in workingByJoin.DefaultIfEmpty()
                        join p in _context.Processes on wb.ProcessId equals p.ProcessId into processJoin
                        from p in processJoin.DefaultIfEmpty()

                        where (wb.EmployeeId == null && e.Status == "Available") 
                        || (p.Date >= dateValue && (p.StarTime > endTimeSpan && p.EndTime < startTimeSpan)) 
                        && e.Status == "Available"
                        select new Employee
                        {
                            EmployeeId = e.EmployeeId,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            Phone = e.Phone,
                            AccountId = e.AccountId,
                            Status = e.Status,
                            EndDate = e.EndDate,
                            StartDate = e.StartDate
                            
                        };
            return await query.ToListAsync();
        }
    }
}
