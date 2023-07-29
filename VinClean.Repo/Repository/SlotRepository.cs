using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface ISlotRepository
    {
        Task<ICollection<Slot>> GetSlotList();
        Task<Slot> GetSlotById(int id);
        Task<bool> UpdateSlot(Slot customer);
        Task<bool> AddSlot(Slot slot);
        Task<bool> DeleteSlot (int id);
    }
    public class SlotRepository : ISlotRepository
    {
        private readonly ServiceAppContext _context;
        public SlotRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<ICollection<Slot>> ISlotRepository.GetSlotList()
        {
            return await _context.Slots.ToListAsync();
        }
        async Task<Slot> ISlotRepository.GetSlotById(int id)
        {
            return await _context.Slots.FirstOrDefaultAsync(a => a.SlotId == id);
        }

        async Task<bool> ISlotRepository.UpdateSlot(Slot type)
        {
            _context.Slots.Update(type);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> ISlotRepository.AddSlot(Slot slot)
        {
            _context.Slots.Add(slot);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> ISlotRepository.DeleteSlot(int id)
        {
            var _exisitngType = await _context.Slots.FirstOrDefaultAsync(a => a.SlotId == id);
            if (_exisitngType != null)
            {
                _context.Slots.Remove(_exisitngType);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
