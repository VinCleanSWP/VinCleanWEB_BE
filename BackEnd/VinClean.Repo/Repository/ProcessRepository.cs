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
        Task<ICollection<ProcessModeDTO>> GetProcesslist();
        Task<Process> GetProcessById(int id);
        Task<ProcessModeDTO> GetAllInfoById(int id);
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

        async Task<ICollection<ProcessModeDTO>> IProcessRepository.GetProcesslist()
        {
            var query = from p in _context.Processes
                        join m in _context.ProcessDetails on p.ProcessId equals m.ProcessId into mGroup
                        from m in mGroup.DefaultIfEmpty()
                        join s in _context.Services on m.ServiceId equals s.ServiceId into sGroup
                        from s in sGroup.DefaultIfEmpty()
                        join ps in _context.ProcessSlots on p.ProcessId equals ps.ProcessId into psGroup
                        from ps in psGroup.DefaultIfEmpty()
                        join c in _context.Customers on p.CustomerId equals c.CustomerId into cGroup
                        from c in cGroup.DefaultIfEmpty()
                        join ac in _context.Accounts on c.AccountId equals ac.AccountId into acGroup
                        from ac in acGroup.DefaultIfEmpty()
                        join wb in _context.WorkingBies on p.ProcessId equals wb.ProcessId into wbGroup
                        from wb in wbGroup.DefaultIfEmpty()
                        join e in _context.Employees on wb.EmployeeId equals e.EmployeeId into eGroup
                        from e in eGroup.DefaultIfEmpty()
                        join ac1 in _context.Accounts on e.AccountId equals ac1.AccountId into ac1Group
                        from ac1 in ac1Group.DefaultIfEmpty()
                        join t in _context.Types on s.TypeId equals t.TypeId into tGroup
                        from t in tGroup.DefaultIfEmpty()
                        where p.IsDeleted == false
                        select new ProcessModeDTO
                        {
                            ProcessId = p.ProcessId,
                            CustomerId = c.CustomerId,
                            AccountId = c.AccountId,
                            Name = ac.Name,
                            Phone = p.Phone,
                            Address = p.Address,
                            Email = ac.Email,
                            Status = p.Status,
                            Date = (DateTime)p.Date,
                            Note = p.Note,
                            IsDeleted = p.IsDeleted,
                            StartWorking = p.StartWorking,
                            EndWorking = p.EndWorking,
                            CreatedDate = p.CreatedDate,
                            ModifiedBy = p.ModifiedBy,
                            ServiceId = s.ServiceId,
                            ServiceName = s.Name,
                            CostPerSlot = s.Cost,
                            MinimalSlot = s.MinimalSlot,
                            TypeId = t.TypeId,
                            TypeName = t.Type1,
                            StartTime = p.StarTime,
                            EndTime = p.EndTime,
                            Price = p.Price,
                            PointUsed = p.PointUsed,
                            AccountImage = ac.Img,
                            EmployeeImage = ac1.Img,
                            EmployeeId = e.EmployeeId,
                            EmployeeAccountId = e.AccountId,
                            EmployeeName = ac1.Name,
                            EmployeePhone = e.Phone,
                            EmployeeEmail = ac1.Email
                        };
            return await query.ToListAsync();
        }
        async Task<Process> IProcessRepository.GetProcessById(int id)
        {
            return await _context.Processes.FirstOrDefaultAsync(p => p.ProcessId == id);
        }
        async Task<ProcessModeDTO> IProcessRepository.GetAllInfoById(int id)
        {
            /*return await _context.Processes.Include(e=>e.ProcessSlots).ThenInclude(p=>p.Slot).FirstOrDefaultAsync(e=>e.ProcessId == id);*/
            /*return await _context.Processes.Where(e=>e.ProcessId == id).Include(e=> e.Customer).ThenInclude(c => c.Account)
               .Include(e=>e.ProcessSlots).ThenInclude(ps=>ps.Slot)
               .Include(e=>e.ProcessDetails).ThenInclude(pd=>pd.Service).ThenInclude(s=>s.Type)
               .Include(e=>e.WorkingBies).ThenInclude(wb=>wb.Employee).FirstOrDefaultAsync();*/

            var query = from p in _context.Processes
                        join m in _context.ProcessDetails on p.ProcessId equals m.ProcessId into mGroup
                        from m in mGroup.DefaultIfEmpty()
                        join s in _context.Services on m.ServiceId equals s.ServiceId into sGroup
                        from s in sGroup.DefaultIfEmpty()
                        join ps in _context.ProcessSlots on p.ProcessId equals ps.ProcessId into psGroup
                        from ps in psGroup.DefaultIfEmpty()
                        join c in _context.Customers on p.CustomerId equals c.CustomerId into cGroup
                        from c in cGroup.DefaultIfEmpty()
                        join ac in _context.Accounts on c.AccountId equals ac.AccountId into acGroup
                        from ac in acGroup.DefaultIfEmpty()
                        join wb in _context.WorkingBies on p.ProcessId equals wb.ProcessId into wbGroup
                        from wb in wbGroup.DefaultIfEmpty()
                        join e in _context.Employees on wb.EmployeeId equals e.EmployeeId into eGroup
                        from e in eGroup.DefaultIfEmpty()
                        join ac1 in _context.Accounts on e.AccountId equals ac1.AccountId into ac1Group
                        from ac1 in ac1Group.DefaultIfEmpty()
                        join t in _context.Types on s.TypeId equals t.TypeId into tGroup
                        from t in tGroup.DefaultIfEmpty()
                        where p.ProcessId == id && p.IsDeleted == false
                        select new ProcessModeDTO
                        {
                            ProcessId = p.ProcessId,
                            CustomerId = c.CustomerId,
                            AccountId = c.AccountId,
                            Name = ac.Name,
                            Phone = c.Phone,
                            Address = c.Address,
                            Status = p.Status,
                            Email = ac.Email,
                            Date = (DateTime)p.Date,
                            Note = p.Note,
                            IsDeleted = p.IsDeleted,
                            StartWorking = p.StartWorking,
                            EndWorking = p.EndWorking,
                            CreatedDate = p.CreatedDate,
                            ModifiedBy = p.ModifiedBy,
                            ServiceId = s.ServiceId,
                            ServiceName = s.Name,
                            CostPerSlot = s.Cost,
                            MinimalSlot = s.MinimalSlot,
                            TypeId = t.TypeId,
                            TypeName = t.Type1,
                            StartTime = p.StarTime,
                            EndTime = p.EndTime,
                            Price = p.Price,
                            PointUsed = p.PointUsed,
                            AccountImage = ac.Img,
                            EmployeeImage = ac1.Img,
                            EmployeeId = e.EmployeeId,
                            EmployeeAccountId = e.AccountId,
                            EmployeeName = ac1.Name,
                            EmployeePhone = e.Phone,
                            EmployeeEmail = ac1.Email
                        };
            return await query.FirstOrDefaultAsync();

            /* var query = _context.Processes
         .Where(p => p.ProcessId == id)
         .Select(p => new ProcessModeDTO
         {
             ProcessId = p.ProcessId,
             CustomerId = p.Customer.CustomerId,
             AccountId = p.Customer.Account.AccountId,
             AccountName = p.Customer.Account.Name,
             CustomerPhone = p.Customer.Phone,
             CustomerAddress = p.Customer.Address,
             ProcessStatus = p.Status,
             ProcessNote = p.Note,
             IsDeleted = p.IsDeleted,
             CreatedDate = p.CreatedDate,
             ModifiedBy = p.ModifiedBy,
             ServiceId = p.ProcessDetails.FirstOrDefault().Service.ServiceId,
             ServiceName = p.ProcessDetails.FirstOrDefault().Service.Name,
             CostPerSlot = p.ProcessDetails.FirstOrDefault().Service.CostPerSlot,
             MinimalSlot = p.ProcessDetails.FirstOrDefault().Service.MinimalSlot,
             TypeId = p.ProcessDetails.FirstOrDefault().Service.Type.TypeId,
             TypeName = p.ProcessDetails.FirstOrDefault().Service.Type.Type1,
             SlotId = p.ProcessSlots.FirstOrDefault().Slot.SlotId,
             SlotName = p.ProcessSlots.FirstOrDefault().Slot.SlotName,
             DayOfWeek = p.ProcessSlots.FirstOrDefault().Slot.DayOfweek,
             StartTime = p.ProcessSlots.FirstOrDefault().Slot.StartTime,
             EndTime = p.ProcessSlots.FirstOrDefault().Slot.EndTime,
             TotalMoney = p.Customer.TotalMoney,
             TotalPoint = p.Customer.TotalPoint,
             Employee = new EmployeeInfoDTO
             {
                 EmployeeId = p.WorkingBies.FirstOrDefault().Employee.EmployeeId,
                 AccountId = p.WorkingBies.FirstOrDefault().Employee.Account.AccountId,
                 AccountName = p.WorkingBies.FirstOrDefault().Employee.Account.Name,
                 Phone = p.WorkingBies.FirstOrDefault().Employee.Phone,
                 Email = p.WorkingBies.FirstOrDefault().Employee.Account.Email,
                 Image = p.WorkingBies.FirstOrDefault().Employee.Account.Image
             },
             CustomerImage = p.Customer.Account.Image
         });*/

            /*        }*/
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
