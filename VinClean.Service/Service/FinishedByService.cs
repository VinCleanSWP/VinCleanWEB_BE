using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Employee;

namespace VinClean.Service.Service
{
    public interface IFinishedByService
    {
        Task<ServiceResponse<List<FinishedByDTO>>> GetFinishedByList();
        Task<ServiceResponse<FinishedByDTO>> GetFinishedById(int id);
        Task<ServiceResponse<FinishedByDTO>> AddFinishedBy(FinishedByDTO request);
        Task<ServiceResponse<FinishedByDTO>> UpdateFinishedBy(FinishedByDTO request);
        Task<ServiceResponse<FinishedByDTO>> DeleteFinishedBy(int id);

    }
    public class FinishedByService : IFinishedByService
    {
        private readonly IFinishedByRepository _repository;
        private readonly IMapper _mapper;

        public FinishedByService(IFinishedByRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<FinishedByDTO>> AddFinishedBy(FinishedByDTO request)
        {
            ServiceResponse<FinishedByDTO> _response = new();
            try
            {
                FinshedBy _newFinishedBy = new FinshedBy()
                {
                    EmployeeId = request.EmployeeId,
                    OrderId = request.OrderId,
                };

                if (!await _repository.AddFinishedBy(_newFinishedBy))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<FinishedByDTO>(_newFinishedBy);
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

        public async Task<ServiceResponse<FinishedByDTO>> DeleteFinishedBy(int id)
        {
            ServiceResponse<FinishedByDTO> _response = new();
            try
            {
                var existingFinishedBy = await _repository.GetFinishedById(id);
                if (existingFinishedBy == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (await _repository.DeleteFinishedBy(existingFinishedBy))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<FinishedByDTO>(existingFinishedBy);
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

        public async Task<ServiceResponse<FinishedByDTO>> GetFinishedById(int id)
        {
            ServiceResponse<FinishedByDTO> _response = new();
            try
            {
                var FinishedBy = await _repository.GetFinishedById(id);
                if (FinishedBy == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var FinishedBydto = _mapper.Map<FinishedByDTO>(FinishedBy);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = FinishedBydto;

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

        public async Task<ServiceResponse<List<FinishedByDTO>>> GetFinishedByList()
        {
            ServiceResponse<List<FinishedByDTO>> _response = new();
            try
            {
                var ListFinishedBy = await _repository.GetFinishedByList();
                var ListFinishedByDTO = new List<FinishedByDTO>();
                foreach (var FinishedBy in ListFinishedBy)
                {
                    ListFinishedByDTO.Add(_mapper.Map<FinishedByDTO>(FinishedBy));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListFinishedByDTO;
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

        public async Task<ServiceResponse<FinishedByDTO>> UpdateFinishedBy(FinishedByDTO request)
        {
            ServiceResponse<FinishedByDTO> _response = new();
            try
            {
                var existingFinishedBy = await _repository.GetFinishedById(request.OrderId);
                if (existingFinishedBy == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                // cac gia trị cho sua
                existingFinishedBy.OrderId = request.OrderId;
                existingFinishedBy.EmployeeId = request.EmployeeId;


                if (!await _repository.UpdateFinishedBy(existingFinishedBy))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _FinishedByDTO = _mapper.Map<FinishedByDTO>(existingFinishedBy);
                _response.Success = true;
                _response.Data = _FinishedByDTO;
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
