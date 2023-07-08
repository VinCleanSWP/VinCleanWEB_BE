using AutoMapper;
using MailKit.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.CustomerResponse;
using VinClean.Service.DTO.Employee;
using VinClean.Service.DTO.Order;
using VinClean.Service.DTO.Rating;

// Pass data from Repo to Controller

namespace VinClean.Service.Service
{
    public interface IRatingService
    {
        Task<ServiceResponse<List<RatingModelDTO>>> GetRatingList();
        Task<ServiceResponse<List<RatingDTO>>> GetRatingByService(int id);
        Task<ServiceResponse<RatingDTO>> GetRatingById(int id);
        Task<ServiceResponse<RatingDTO>> AddRating(AddRateDTO Rating);
        Task<ServiceResponse<RatingDTO>> UpdateRating(RatingDTO Rating);
        Task<ServiceResponse<RatingDTO>> DeleteRating(int id);
    }
    public class RatingService : IRatingService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        public readonly IMapper _mapper;
        public RatingService(ICustomerRepository customerRepository, IRatingRepository ratingRepository, IServiceRepository serviceRepository, IMapper mapper,
            IOrderDetailRepository orderDetailRepository, IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _ratingRepository = ratingRepository;
            _serviceRepository = serviceRepository;
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        // Get Rating List
        public async Task<ServiceResponse<List<RatingModelDTO>>> GetRatingList()
        {
            ServiceResponse<List<RatingModelDTO>> _response = new();
            try
            {
                var listRating = await _ratingRepository.GetRatinglist();
                var listRatingDTO = new List<RatingModelDTO>();
                foreach (var rating in listRating)
                {
                    listRatingDTO.Add(_mapper.Map<RatingModelDTO>(rating));
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

        // Get Rating List By ServiceID
        async Task<ServiceResponse<List<RatingDTO>>> IRatingService.GetRatingByService(int id)
        {
            ServiceResponse<List<RatingDTO>> _response = new();
            try
            {
                var ratingList = await _ratingRepository.GetRatingByService(id);
                if (ratingList == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var ratingDTO = new List<RatingDTO>();
                foreach (var rating in ratingList)
                {
                    ratingDTO.Add(_mapper.Map<RatingDTO>(rating));
                }
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

        // Get Rating By ID
        public async Task<ServiceResponse<RatingDTO>> GetRatingById(int id)
        {
            ServiceResponse<RatingDTO> _response = new();
            try
            {
                var rating = await _ratingRepository.GetRatingById(id);
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

        // Add Rating To Ordered Service
        public async Task<ServiceResponse<RatingDTO>> AddRating(AddRateDTO request)
        {
            ServiceResponse<RatingDTO> _response = new();
            
            try
            {
                var existingOrder = await _orderRepository.GetInfoOrderById(request.OrderId);
                //var existingRating = await _ratingRepository.GetRatingById(request.RateId);
                if (existingOrder == null)
                {
                    _response.Success = false;
                    _response.Message = "OrderId in order does not exist";
                    _response.Data = null;
                    return _response;
                }
                var existingOrderDetail = await _orderDetailRepository.GetOrderDetailById(request.OrderId);
                if (existingOrderDetail == null)
                {
                    _response.Success = false;
                    _response.Message = "OrderId in order detail does not exist";
                    _response.Data = null;
                    return _response;
                }

                Rating _newRating = new Rating()
                {
                    ServiceId = existingOrder.ServiceId,
                    Rate = request.Rate,
                    Comment = request.Comment,
                    CreatedDate = DateTime.Now,
                    CustomerId = existingOrder.CustomerId,
                    IsDeleted = false,
                };

                if (!await _ratingRepository.AddRating(_newRating))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }


                var or = await _orderRepository.GetInfoOrderById(request.OrderId);
                if (or == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }


                existingOrderDetail.OrderId = or.OrderId;
                existingOrderDetail.ServiceId = or.ServiceId;
                existingOrderDetail.RateId = _newRating.RateId;
                existingOrderDetail.Slot = 1;
                existingOrderDetail.Total = (decimal)or.Total;
                existingOrderDetail.StartWorking = (TimeSpan)or.StartWorking;
                existingOrderDetail.EndWorking = (TimeSpan)or.EndWorking;
                
                

                if (!await _orderDetailRepository.UpdateOrderDetail(existingOrderDetail))
                {
                    _response.Success = false;
                    _response.Message = "Update Order Detail Fail";
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

        // Update Existing Rating
        public async Task<ServiceResponse<RatingDTO>> UpdateRating(RatingDTO request)
        {
            ServiceResponse<RatingDTO> _response = new();
            try
            {
                var existingRating = await _ratingRepository.GetRatingById(request.RateId);
                if (existingRating == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingRating.Rate = request.Rate;
                existingRating.Comment = request.Comment;
                existingRating.ModifiedDate = DateTime.Now;

                if (!await _ratingRepository.UpdateRating(existingRating))
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

        // Delete Existing Rating
        public async Task<ServiceResponse<RatingDTO>> DeleteRating(int id)
        {
            ServiceResponse<RatingDTO> _response = new();
            try
            {
                var existingRating = await _ratingRepository.GetRatingById(id);
                if (existingRating == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _ratingRepository.DeleteRating(existingRating))
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
