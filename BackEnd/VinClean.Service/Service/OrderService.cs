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
        Task<ServiceResponse<List<OrderModelDTO>>> GetOrderList();
        Task<ServiceResponse<OrderModelDTO>> GetOrderById(int id);
        Task<ServiceResponse<NewOderDTO>> AddOrder(NewOderDTO request);
        Task<ServiceResponse<OrderDTO>> UpdateOrder(OrderDTO request);
        Task<ServiceResponse<OrderDTO>> DeleteOrder(int id);

    }
    public class OrderService : IOrderService
    {
        private readonly IProcessRepository _processRepository;
        private readonly IOrderDetailRepository _odRepository;
        private readonly IFinishedByRepository _fbRepository;
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repository, IMapper mapper, IOrderDetailRepository odRepository, IFinishedByRepository fbRepository, IProcessRepository processRepository)
        {
            _odRepository = odRepository;
            _odRepository = odRepository;
            _fbRepository = fbRepository;
            _repository = repository;
            _mapper = mapper;
            _processRepository = processRepository;
        }
        public async Task<ServiceResponse<NewOderDTO>> AddOrder(NewOderDTO request)
        {
            ServiceResponse<NewOderDTO> _response = new();
            try
            {
                Order _newOrder = new Order()
                {
                    CustomerId = request.CustomerId,
                    Note = request.Note,
                    Total = request.Total,
                    OrderDate = DateTime.Now,
                    DateWork = request.DateWork,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,

                };
                var check1 = await _repository.AddOrder(_newOrder);
                FinshedBy _finshedBy = new FinshedBy()
                {
                    OrderId = _newOrder.OrderId,
                    EmployeeId = request.EmployeeId
                };
                var check2 = await _fbRepository.AddFinishedBy(_finshedBy);

                OrderDetail _oderDetail = new OrderDetail()
                {
                    OrderId = _finshedBy.OrderId,
                    ServiceId = request.ServiceId,
                    StartWorking = request.StartWorking,
                    EndWorking = request.EndWorking
                };
                var check3 = await _odRepository.AddOrderDetail(_oderDetail);

                Process _updateIsDeleted = new Process()
                {
                    IsDeleted = true
                };
                var check4 = await _processRepository.UpdateProcess(_updateIsDeleted);


                if (!check1&&!check2&&!check3&&check4)
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<NewOderDTO>(_newOrder);
                _response.Message = "Created";

        }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message)
    };
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

        public async Task<ServiceResponse<OrderModelDTO>> GetOrderById(int id)
        {
            ServiceResponse<OrderModelDTO> _response = new();
            try
            {
                var order = await _repository.GetInfoOrderById(id);
                if (order == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var orderdto = _mapper.Map<OrderModelDTO>(order);
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

        public async Task<ServiceResponse<List<OrderModelDTO>>> GetOrderList()
        {
            ServiceResponse<List<OrderModelDTO>> _response = new();
            try
            {
                var ListOrder = await _repository.GetOrderList();
                var ListOrderDTO = new List<OrderModelDTO>();
                foreach (var order in ListOrder)
                {
                    ListOrderDTO.Add(_mapper.Map<OrderModelDTO>(order));
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

        
    }
}
