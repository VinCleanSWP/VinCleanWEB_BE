using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IProcessImageRepository
    {
        Task<ICollection<ProcessImage>> ProcessImageList();
        Task<ICollection<ProcessImage>> ProcessImageListByProcessId(int id);
        Task<ProcessImage> ProcessImageById(int id);
        Task<bool> UpdateProcessImage(ProcessImage customer);
        Task<bool> AddProcessImage(ProcessImage slot);
        Task<bool> DeleteProcessImage(ProcessImage workingBy);
    }
    public class ProcessImageRepository : IProcessImageRepository
    {
        private readonly ServiceAppContext _context;
        public ProcessImageRepository(ServiceAppContext context)
        {
            _context = context;
        }
        async Task<ICollection<ProcessImage>> IProcessImageRepository.ProcessImageList()
        {
            return await _context.ProcessImages.ToListAsync();
        }
        async Task<ICollection<ProcessImage>> IProcessImageRepository.ProcessImageListByProcessId(int id)
        {
            return await _context.ProcessImages.Where(e => e.ProcessId == id).ToListAsync();
        }

        async Task<ProcessImage> IProcessImageRepository.ProcessImageById(int id)
        {
            return await _context.ProcessImages.FirstOrDefaultAsync(a => a.Id == id);
        }

        async Task<bool> IProcessImageRepository.UpdateProcessImage(ProcessImage processImage)
        {
            _context.ProcessImages.Update(processImage);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IProcessImageRepository.AddProcessImage(ProcessImage processImage)
        {
            _context.ProcessImages.Add(processImage);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
        async Task<bool> IProcessImageRepository.DeleteProcessImage(ProcessImage processImage)
        {
            _context.ProcessImages.Remove(processImage);
            return await _context.SaveChangesAsync() > 0 ? true : false; ;
        }
    }
}
