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
    public interface IRatingRepository
    {
        Task<ICollection<Rating>> GetRatinglist();
        Task<Rating> GetRatingById(int id);
        Task<bool> AddRating(Rating rating);
        Task<bool> UpdateRating(Rating rating);
        Task<bool> DeleteRating(Rating rating);
    }
    public class RatingRepository : IRatingRepository
    {
        private readonly ServiceAppContext _context;
        public RatingRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<ICollection<Rating>> IRatingRepository.GetRatinglist()
        {
            return await _context.Ratings.ToListAsync();
        }
        async Task<Rating> IRatingRepository.GetRatingById(int id)
        {
            return await _context.Ratings.FirstOrDefaultAsync(r => r.RateId == id);
        }

        async Task<bool> IRatingRepository.AddRating(Rating rating)
        {
            _context.Ratings.Add(rating);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IRatingRepository.UpdateRating(Rating rating)
        {
            _context.Ratings.Update(rating);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IRatingRepository.DeleteRating(Rating rating)
        {
            _context.Ratings.Remove(rating);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
