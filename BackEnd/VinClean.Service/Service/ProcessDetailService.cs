using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.CustomerResponse;
using VinClean.Service.DTO.Process;

namespace VinClean.Service.Service
{
    public interface IProcessDetailService
    {
        Task<ServiceResponse<List<ProcessDetailDTO>>> GetPD();
        Task<ServiceResponse<ProcessDetailDTO>> GetPDById(int id);
        Task<ServiceResponse<ProcessDetailDTO>> CreatePD(ProcessDetailDTO processDetailDTO);
        Task<ServiceResponse<ProcessDetailDTO>> UpdatePD(ProcessDetailDTO processDetailDTO);
        Task<ServiceResponse<ProcessDetailDTO>> DeletePD(int id);
    }

    public class ProcessDetailService : IProcessDetailService
    {
        private readonly IProcessDetailRepository _pdRepository;
        private readonly IMapper _mapper;
        private readonly IServiceRepository _serviceRepository;

        public ProcessDetailService(IProcessDetailRepository pdRepository, IMapper mapper, IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
            _pdRepository = pdRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<ProcessDetailDTO>>> GetPDList()
        {
            ServiceResponse<List<ProcessDetailDTO>> _response = new();
            try
            {
                var listProcessDetail = await _pdRepository.GetPDList();
                var listProcessDetailDTO = new List<ProcessDetailDTO>();
                foreach (var processDetail in listProcessDetail)
                {
                    var processDetailDTO = _mapper.Map<ProcessDetailDTO>(processDetail);
                    listProcessDetailDTO.Add(processDetailDTO);

                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = listProcessDetailDTO;
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

        public async Task<ServiceResponse<ProcessDetailDTO>> GetPDById(int id)
        {
            ServiceResponse<ProcessDetailDTO> _response = new();
            try
            {
                var processDetail = await _pdRepository.GetPDById(id);
                if (processDetail == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = _mapper.Map<ProcessDetailDTO>(processDetail);


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

        public async Task<ServiceResponse<ProcessDetailDTO>> CreatePD(ProcessDetailDTO request)
        {
            ServiceResponse<ProcessDetailDTO> _response = new();
            try
            {
                ProcessDetail _newProcessDetail = new ProcessDetail()
                {
                    ProcessId = request.ProcessId,
                    ServiceId = request.ServiceId,
                    
                };

                if (!await _pdRepository.AddPD(_newProcessDetail))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<ProcessDetailDTO>(_newProcessDetail);
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

        public async Task<ServiceResponse<ProcessDetailDTO>> UpdatePD(ProcessDetailDTO request)
        {
            ServiceResponse<ProcessDetailDTO> _response = new();
            try
            {
                var existingPd = await _pdRepository.GetPDById(request.ProcessId);
                if (existingPd == null)
                {
                    _response.Message = "NotFound";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }
                var _newProcessDetail = await _pdRepository.GetPDById(request.ProcessId);
                

                if (!await _pdRepository.UpdatePD(existingPd))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<ProcessDetailDTO>(existingPd);
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
        public async Task<ServiceResponse<ProcessDetailDTO>> DeletePD(int id)
        {
            ServiceResponse<ProcessDetailDTO> _response = new();
            try
            {
                var existingPd = await _pdRepository.GetPDById(id);
                if (existingPd == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _pdRepository.DeletePD(existingPd))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _processDetailDTO = _mapper.Map<ProcessDetailDTO>(existingPd);
                _response.Success = true;
                _response.Data = _processDetailDTO;
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
    }
}
