using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Rating;

namespace VinClean.Service.Service
{
    public interface IRatingService
    {
        Task<ServiceResponse<List<RatingDTO>>> GetRatingList();
        Task<ServiceResponse<RatingDTO>> GetRatingById(int id);
        Task<ServiceResponse<RatingDTO>> AddRating(RatingDTO Rating);
        Task<ServiceResponse<RatingDTO>> UpdateRating(RatingDTO Rating);
        Task<ServiceResponse<RatingDTO>> DeleteRating(int id);
    }
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _repository;
        public readonly IMapper _mapper;
        public RatingService(IRatingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<RatingDTO>>> GetRatingList()
        {
            ServiceResponse<List<RatingDTO>> _response = new();
            try
            {
                var listRating = await _repository.GetRatinglist();
                var listRatingDTO = new List<RatingDTO>();
                foreach (var Rating in listRating)
                {
                    listRatingDTO.Add(_mapper.Map<RatingDTO>(Rating));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = listRatingDTO;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<RatingDTO>> GetRatingById(int id)
        {
            ServiceResponse<RatingDTO> _response = new();
            try
            {
                var rating = await _repository.GetRatingById(id);
                if (rating == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var ratingDTO = _mapper.Map<RatingDTO>(rating);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ratingDTO;

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<RatingDTO>> AddRating(RatingDTO request)
        {
            ServiceResponse<RatingDTO> _response = new();
            try
            {
                Rating _newRating = new Rating()
                {
                    //RateId = request.RateId,
                    //ServiceId = request.ServiceId,
                    //CustomerId = request.CustomerId,
                    Rate = request.Rate,
                    Comment = request.Comment,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                };

                if (!await _repository.AddRating(_newRating))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<RatingDTO>(_newRating);
                _response.Message = "Rating Created";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }

        public async Task<ServiceResponse<RatingDTO>> UpdateRating(RatingDTO request)
        {
            ServiceResponse<RatingDTO> _response = new();
            try
            {
                var existingRating = await _repository.GetRatingById(request.RateId);
                if (existingRating == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingRating.Rate = request.Rate;
                existingRating.Comment = request.Comment;
                existingRating.IsDeleted = request.IsDeleted;
                //existingRating.ModifiedDate = DateTime.Now;
                //existingRating.ModifiedBy = request.ModifiedBy;

                if (!await _repository.UpdateRating(existingRating))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _ratingDTO = _mapper.Map<RatingDTO>(existingRating);
                _response.Success = true;
                _response.Data = _ratingDTO;
                _response.Message = "Rating Updated";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<RatingDTO>> DeleteRating(int id)
        {
            ServiceResponse<RatingDTO> _response = new();
            try
            {
                var existingRating = await _repository.GetRatingById(id);
                if (existingRating == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _repository.DeleteRating(existingRating))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _ratingDTO = _mapper.Map<RatingDTO>(existingRating);
                _response.Success = true;
                _response.Data = _ratingDTO;
                _response.Message = "Rating Deleted";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }
    }
}
