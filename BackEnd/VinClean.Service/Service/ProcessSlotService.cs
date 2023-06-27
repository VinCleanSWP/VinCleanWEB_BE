using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Process;
using VinClean.Service.DTO.Slot;

namespace VinClean.Service.Service
{
    public interface IProcessSlotService
    {
        Task<ServiceResponse<List<ProcessSlotDTO>>> GetPS();
        Task<ServiceResponse<ProcessSlotDTO>> GetPSById(int id);
        Task<ServiceResponse<ProcessSlotDTO>> CreatePS(ProcessSlotDTO processSlotDTO);
        Task<ServiceResponse<ProcessSlotDTO>> UpdatePS(ProcessSlotDTO processSlotDTO);
        Task<ServiceResponse<ProcessSlotDTO>> DeletePS(int id);
    }
    public class ProcessSlotService : IProcessSlotService
    {
        private readonly IProcessSlotRepository _repository;
        public readonly IMapper _mapper;

        public ProcessSlotService(IProcessSlotRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<ProcessSlotDTO>>> GetPS()
        {
            ServiceResponse<List<ProcessSlotDTO>> _response = new();
            try
            {
                var processSlots = await _repository.GetPSList();
                var processSlotDTOs = new List<ProcessSlotDTO>();

                foreach (var processSlot in processSlots)
                {
                    var processSlotDTO = _mapper.Map<ProcessSlotDTO>(processSlot);
                    processSlotDTO.Process = _mapper.Map<ProcessDTO>(processSlot.Process);
                    processSlotDTO.Slot = _mapper.Map<SlotDTO>(processSlot.Slot);

                    processSlotDTOs.Add(processSlotDTO);
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = processSlotDTOs;
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

        public async Task<ServiceResponse<ProcessSlotDTO>> GetPSById(int id)
        {
            ServiceResponse<ProcessSlotDTO> _response = new();
            try
            {
                var process = await _repository.GetPSById(id);
                if (process == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var processSlotDTO = _mapper.Map<ProcessSlotDTO>(process);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = processSlotDTO;

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

        public async Task<ServiceResponse<ProcessSlotDTO>> CreatePS(ProcessSlotDTO request)
        {
            ServiceResponse<ProcessSlotDTO> _response = new();
            try
            {
                ProcessSlot _newProcess = new ProcessSlot()
                {
                    ProcessId = request.ProcessId,
                    SlotId = request.SlotId,
                };

                if (!await _repository.AddPS(_newProcess))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<ProcessSlotDTO>(_newProcess);
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

        public async Task<ServiceResponse<ProcessSlotDTO>> UpdatePS(ProcessSlotDTO request)
        {
            ServiceResponse<ProcessSlotDTO> _response = new();
            try
            {
                var existingProcess = await _repository.GetPSById(request.ProcessId);
                if (existingProcess == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingProcess.ProcessId = request.ProcessId;
                existingProcess.SlotId = request.SlotId;

                if (!await _repository.UpdatePS(existingProcess))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _processSlotDTO = _mapper.Map<ProcessSlotDTO>(existingProcess);
                _response.Success = true;
                _response.Data = _processSlotDTO;
                _response.Message = "Process Updated";

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

        public async Task<ServiceResponse<ProcessSlotDTO>> DeletePS(int id)
        {
            ServiceResponse<ProcessSlotDTO> _response = new();
            try
            {
                var existingProcess = await _repository.GetPSById(id);
                if (existingProcess == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _repository.DeletePS(existingProcess))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _processSlotDTO = _mapper.Map<ProcessSlotDTO>(existingProcess);
                _response.Success = true;
                _response.Data = _processSlotDTO;
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
