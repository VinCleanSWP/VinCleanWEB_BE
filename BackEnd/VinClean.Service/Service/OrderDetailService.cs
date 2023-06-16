using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Service.DTO;
using AutoMapper;
using VinClean.Repo.Repository;
using VinClean.Service.DTO.Order;
using VinClean.Repo.Models;

namespace VinClean.Service.Service
{
    public interface IOrderDetailService
    {
        Task<ServiceResponse<List<OrderDetailDTO>>> GetOrderDetailList();
        Task<ServiceResponse<OrderDetailDTO>> GetOrderDetailById(int id);
        Task<ServiceResponse<OrderDetailDTO>> AddOrderDetail(OrderDetailDTO request);
        Task<ServiceResponse<OrderDetailDTO>> UpdateOrderDetail(OrderDetailDTO request);
        Task<ServiceResponse<OrderDetailDTO>> DeleteOrderDetail(int id);

    }
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _repository;
        private readonly IMapper _mapper;
        public OrderDetailService(IOrderDetailRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }
        public async Task<ServiceResponse<OrderDetailDTO>> AddOrderDetail(OrderDetailDTO request)
        {
            ServiceResponse<OrderDetailDTO> _response = new();
            try
            {
                OrderDetail _newOrderDetail = new OrderDetail()
                {
                    ServiceId = request.ServiceId,
                    Slot = request.Slot,
                    Total = request.Total,
                };

                if (!await _repository.AddOrderDetail(_newOrderDetail))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<OrderDetailDTO>(_newOrderDetail);
                _response.Message = "Created";

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

        public async Task<ServiceResponse<OrderDetailDTO>> DeleteOrderDetail(int id)
        {
            ServiceResponse<OrderDetailDTO> _response = new();
            try
            {
                var existingOrderDetail = await _repository.GetOrderDetailById(id);
                if (existingOrderDetail == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (await _repository.DeleteOrderDetail(existingOrderDetail))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<OrderDetailDTO>(existingOrderDetail);
                _response.Success = true;
                _response.Data = _OrderDTO;
                _response.Message = "Deleted";

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

        public async Task<ServiceResponse<OrderDetailDTO>> GetOrderDetailById(int id)
        {
            ServiceResponse<OrderDetailDTO> _response = new();
            try
            {
                var orderdetail = await _repository.GetOrderDetailById(id);
                if (orderdetail == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var orderdetaildto = _mapper.Map<OrderDetailDTO>(orderdetail);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = orderdetaildto;

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

        public async Task<ServiceResponse<List<OrderDetailDTO>>> GetOrderDetailList()
        {
            ServiceResponse<List<OrderDetailDTO>> _response = new();
            try
            {
                var ListOrderDetail = await _repository.GetOrderDetailList();
                var ListOrderDetailDTO = new List<OrderDetailDTO>();
                foreach (var od in ListOrderDetail)
                {
                    ListOrderDetailDTO.Add(_mapper.Map<OrderDetailDTO>(od));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListOrderDetailDTO;
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

        public async Task<ServiceResponse<OrderDetailDTO>> UpdateOrderDetail(OrderDetailDTO request)
        {
            ServiceResponse<OrderDetailDTO> _response = new();
            try
            {
                var existingOrderDetail = await _repository.GetOrderDetailById(request.OrderId);
                if (existingOrderDetail == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                // cac gia trị cho sua
                existingOrderDetail.Slot = request.Slot;
                existingOrderDetail.Total = request.Total;


                if (!await _repository.UpdateOrderDetail(existingOrderDetail))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDetailDTO = _mapper.Map<OrderDetailDTO>(existingOrderDetail);
                _response.Success = true;
                _response.Data = _OrderDetailDTO;
                _response.Message = "Updated";

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
