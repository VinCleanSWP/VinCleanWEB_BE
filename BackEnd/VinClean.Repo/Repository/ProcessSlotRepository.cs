using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VinClean.Repo.Models;

// lay table

namespace VinClean.Repo.Repository
{
    public interface IProcessSlotRepository
    {
        Task<ICollection<ProcessRequestModel>> GetPSList();
        Task<ProcessRequestModel> GetInfoPSById(int id);
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

        async Task<ICollection<ProcessRequestModel>> IProcessSlotRepository.GetPSList()
        {
            /*return await _context.ProcessSlots.Include(e=>e.OldEmployee)
                .Include(e=>e.Process).ThenInclude(p=>p.Customer)
                .Include(e=>e.NewEmployee)
                .Include(e=>e.CreateByNavigation)
                .ToListAsync();*/
            var query = from ps in _context.ProcessSlots
                        join oe in _context.Employees on ps.OldEmployeeId equals oe.EmployeeId into oeGroup
                        from oe in oeGroup.DefaultIfEmpty()
                        join oeac in _context.Accounts on oe.AccountId equals oeac.AccountId into oeacGroup
                        from oeac in oeacGroup.DefaultIfEmpty()
                        join ne in _context.Employees on ps.NewEmployeeId equals ne.EmployeeId into neGroup
                        from ne in neGroup.DefaultIfEmpty()
                        join neac in _context.Accounts on ne.AccountId equals neac.AccountId into neacGroup
                        from neac in neacGroup.DefaultIfEmpty()
                        join p in _context.Processes on ps.ProcessId equals p.ProcessId into pGroup
                        from p in pGroup.DefaultIfEmpty()
                        join c in _context.Customers on p.CustomerId equals c.CustomerId into cGroup
                        from c in cGroup.DefaultIfEmpty()
                        join pd in _context.ProcessDetails on p.ProcessId equals pd.ProcessId into pdGroup
                        from pd in pdGroup.DefaultIfEmpty()
                        select new ProcessRequestModel
                        {
                            ProcessId = p.ProcessId,
                            CustomerId = (int)p.CustomerId,
                            CustomerName = c.LastName +" "+ c.FirstName,
                            Address = p.Address,
                            AccountId = ps.CreateBy,
                            OldEmployeeId = (int)ps.OldEmployeeId,
                            OldEmployeeName = oeac.Name,
                            OldEmployePhone = oe.Phone,
                            OldEmployeEmail = oeac.Email,
                            OldEmployeImg = oeac.Img,
                            ServiceId = (int)pd.ServiceId,
                            Date = p.Date,
                            StartTime = p.StarTime,
                            EndTime = p.EndTime,
                            NewEmployeeId = (int)ps.NewEmployeeId,
                            NewEmployeeName = neac.Name,
                            NewEmployePhone = ne.Phone,
                            NewEmployeEmail = neac.Email,
                            NewEmployeImg = neac.Img,
                            Status = ps.Satus,
                            Reason = ps.Note
                        };
            return await query.ToListAsync();
                        //join c in _context.Customers on p.CustomerId equals c.CustomerId into cGroup
                        //from c in cGroup.DefaultIfEmpty()
                        //join ac in _context.Accounts on c.AccountId equals ac.AccountId into acGroup
                        //from ac in acGroup.DefaultIfEmpty()

        }

        async Task<ProcessRequestModel> IProcessSlotRepository.GetInfoPSById(int id)
        {
            var query = from ps in _context.ProcessSlots
                        join oe in _context.Employees on ps.OldEmployeeId equals oe.EmployeeId into oeGroup
                        from oe in oeGroup.DefaultIfEmpty()
                        join oeac in _context.Accounts on oe.AccountId equals oeac.AccountId into oeacGroup
                        from oeac in oeacGroup.DefaultIfEmpty()
                        join ne in _context.Employees on ps.NewEmployeeId equals ne.EmployeeId into neGroup
                        from ne in neGroup.DefaultIfEmpty()
                        join neac in _context.Accounts on ne.AccountId equals neac.AccountId into neacGroup
                        from neac in neacGroup.DefaultIfEmpty()
                        join p in _context.Processes on ps.ProcessId equals p.ProcessId into pGroup
                        from p in pGroup.DefaultIfEmpty()
                        join c in _context.Customers on p.CustomerId equals c.CustomerId into cGroup
                        from c in cGroup.DefaultIfEmpty()
                        join pd in _context.ProcessDetails on p.ProcessId equals pd.ProcessId into pdGroup
                        from pd in pdGroup.DefaultIfEmpty()
                        where ps.ProcessId == id
                        select new ProcessRequestModel
                        {
                            ProcessId = p.ProcessId,
                            CustomerId = (int)p.CustomerId,
                            CustomerName = c.LastName + c.FirstName,
                            Address = p.Address,
                            AccountId = ps.CreateBy,
                            OldEmployeeId = (int)ps.OldEmployeeId,
                            OldEmployeeName = oeac.Name,
                            OldEmployePhone = oe.Phone,
                            OldEmployeEmail = oeac.Email,
                            OldEmployeImg = oeac.Img,
                            ServiceId = (int)pd.ServiceId,
                            Date = p.Date,
                            StartTime = p.StarTime,
                            EndTime = p.EndTime,
                            NewEmployeeId = (int)ps.NewEmployeeId,
                            NewEmployeeName = neac.Name,
                            NewEmployePhone = ne.Phone,
                            NewEmployeEmail = neac.Email,
                            NewEmployeImg = neac.Img,
                            Status = ps.Satus,
                            Reason = ps.Note
                        };
            return await query.FirstOrDefaultAsync();
        }
        async Task<ProcessSlot> IProcessSlotRepository.GetPSById(int id)
        {
            return await _context.ProcessSlots.FirstOrDefaultAsync(ps => ps.ProcessId == id);
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
