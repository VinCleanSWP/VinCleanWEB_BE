using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Service.DTO.Order;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Employee;
using AutoMapper;
using VinClean.Repo.Repository;
using VinClean.Repo.Models;

namespace VinClean.Service.Service
{
    public interface IOrderService
    {
        Task<ServiceResponse<List<OrderDTO>>> GetOrderList();
        Task<ServiceResponse<OrderDTO>> GetOrderById(int id);
        Task<ServiceResponse<OrderDTO>> AddOrder(OrderDTO request);
        Task<ServiceResponse<OrderDTO>> UpdateOrder(OrderDTO request);
        Task<ServiceResponse<OrderDTO>> DeleteOrder(int id);

    }
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<OrderDTO>> AddOrder(OrderDTO request)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                Order _newOrder = new Order()
                {
                    CustomerId = request.CustomerId,
                    Note = request.Note,
                    Total = request.Total,
                    OrderDate = DateTime.Now,

                };

                if (!await _repository.AddOrder(_newOrder))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<OrderDTO>(_newOrder);
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

        public async Task<ServiceResponse<OrderDTO>> DeleteOrder(int id)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                var existingOrder = await _repository.GetOrderById(id);
                if (existingOrder == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (await _repository.DeleteOrder(existingOrder))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<OrderDTO>(existingOrder);
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

        public async Task<ServiceResponse<OrderDTO>> GetOrderById(int id)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                var order = await _repository.GetOrderById(id);
                if (order == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var orderdto = _mapper.Map<OrderDTO>(order);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = orderdto;

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

        public async Task<ServiceResponse<List<OrderDTO>>> GetOrderList()
        {
            ServiceResponse<List<OrderDTO>> _response = new();
            try
            {
                var ListOrder = await _repository.GetOrderList();
                var ListOrderDTO = new List<OrderDTO>();
                foreach (var order in ListOrder)
                {
                    ListOrderDTO.Add(_mapper.Map<OrderDTO>(order));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListOrderDTO;
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

        public async Task<ServiceResponse<OrderDTO>> UpdateOrder(OrderDTO request)
        {
            ServiceResponse<OrderDTO> _response = new();
            try
            {
                var existingOrder = await _repository.GetOrderById(request.OrderId);
                if (existingOrder == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                // cac gia trị cho sua
                existingOrder.CustomerId = request.CustomerId;
                existingOrder.Note = request.Note;
                existingOrder.Total = request.Total;

                if (!await _repository.UpdateOrder(existingOrder))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<OrderDTO>(existingOrder);
                _response.Success = true;
                _response.Data = _OrderDTO;
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
