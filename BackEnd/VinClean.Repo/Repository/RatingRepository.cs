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
        Task<ICollection<RatingModelDTO>> GetRatinglist();
        Task<ICollection<Rating>> GetRatingByService(int id);
        Task<Rating> GetRatingById(int id);
        Task<bool> AddRating(Rating rating);
        Task<bool> UpdateRating(Rating rating);
        Task<bool> DeleteRating(Rating rating);
        Task<bool> CheckServiceRating(int serviceId, int customerId);
    }
    public class RatingRepository : IRatingRepository
    {
        private readonly ServiceAppContext _context;
        public RatingRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<ICollection<RatingModelDTO>> IRatingRepository.GetRatinglist()
        {
            //return await _context.Ratings.ToListAsync();

            var list = from s in _context.Services
                       join t in _context.Types on s.TypeId equals t.TypeId into st
                       from subt in st.DefaultIfEmpty()
                        join r in _context.Ratings on s.ServiceId equals r.ServiceId into sr
                        from subr in sr.DefaultIfEmpty()
                        join c in _context.Customers on subr.CustomerId equals c.CustomerId into cr
                        from subc in cr.DefaultIfEmpty()
                        join o in _context.Orders on subc.CustomerId equals o.CustomerId into oc
                        from subo in oc.DefaultIfEmpty()
                        where subo.CustomerId == subc.CustomerId && s.ServiceId == subr.ServiceId
                        select new RatingModelDTO
                        {
                            RateId = subr.RateId,
                            OrderId = subo.OrderId,
                            ServiceName = s.Name,
                            ServiceType = subt.Type1,
                            Note = subo.Note,
                            CustomerName = subc.FirstName,
                            Rate = subr.Rate,
                            Comment = subr.Comment,
                            RatedDate = subr.CreatedDate
                        };

            return await list.ToListAsync();
        }

        async Task<ICollection<Rating>> IRatingRepository.GetRatingByService(int id)
        {
            return await _context.Ratings.Include(s => s.Service).Where(s => s.ServiceId == id).ToListAsync();
        }

        async Task<Rating> IRatingRepository.GetRatingById(int id)
        {
            return await _context.Ratings.FirstOrDefaultAsync(r => r.RateId == id);
        }

        async Task<bool> IRatingRepository.AddRating(Rating rating)
        {
            //var query = from s in _context.Services
            //            join r in _context.Ratings on s.ServiceId equals r.ServiceId into sr
            //            from subr in sr.DefaultIfEmpty()
            //            join c in _context.Customers on subr.CustomerId equals c.CustomerId into cr
            //            from subc in cr.DefaultIfEmpty()
            //            join o in _context.Orders on subc.CustomerId equals o.CustomerId into oc
            //            from subo in oc.DefaultIfEmpty()
            //            where subo.CustomerId == subc.CustomerId && s.ServiceId == subr.ServiceId
            //            select new
            //            {
            //                OrderId = subo.OrderId,
            //                ServiceName = s.Name,
            //                Note = subo.Note,
            //                CustomerFirstName = subc.FirstName,
            //                Rate = subr.Rate,
            //                Comment = subr.Comment,
            //                RatingCreatedDate = subr.CreatedDate
            //            }; con ChatBot lo nay bay dai nen ko xai khuc nay

            _context.Entry(rating).State = EntityState.Added;
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

        async Task<bool> IRatingRepository.CheckServiceRating(int serviceId, int customerId)
        {
            //return await _context.Ratings.Include(s => s.Service).FirstAsync(OrderDetail)
            //    .Include(od => od.OrderDetail).Include("Orders")
            //    .Where(od => od.ServiceId == serviceId).FirstOrDefaultAsync();


            var check = from r in _context.Ratings
                        join s in _context.Services on r.ServiceId equals s.ServiceId
                        join od in _context.OrderDetails on s.ServiceId equals od.ServiceId
                        join o in _context.Orders on od.OrderId equals o.OrderId
                        join c in _context.Customers on o.CustomerId equals c.CustomerId
                        where o.CustomerId == customerId && od.ServiceId == serviceId
                        select od.ServiceId;
            return await check.AnyAsync();
        }
    }
}
