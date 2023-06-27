using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Repository;
using VinClean.Repo.Models;
using VinClean.Service.DTO;
using System.ComponentModel;

// Pass data from Repo to Controller

namespace VinClean.Service.Service
{
    public interface IProcessService
    {
        Task<ServiceResponse<List<ProcessDTO>>> GetProcessList();
        Task<ServiceResponse<ProcessDTO>> GetProcessById(int id);
        Task<ServiceResponse<ProcessDTO>> AddProcess(ProcessDTO process);
        Task<ServiceResponse<ProcessDTO>> UpdateProcess(ProcessDTO process);
        Task<ServiceResponse<ProcessDTO>> DeleteProcess(int id);
    }

    public class ProcessService : IProcessService
    {
        private readonly IProcessRepository _repository;
        public readonly IMapper _mapper;
        public ProcessService(IProcessRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<ProcessDTO>>> GetProcessList()
        {
            ServiceResponse<List<ProcessDTO>> _response = new();
            try
            {
                var listProcess = await _repository.GetProcesslist();
                var listProcessDTO = new List<ProcessDTO>();
                foreach (var process in listProcess)
                {
                    listProcessDTO.Add(_mapper.Map<ProcessDTO>(process));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = listProcessDTO;
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

        public async Task<ServiceResponse<ProcessDTO>> GetProcessById(int id)
        {
            ServiceResponse<ProcessDTO> _response = new();
            try
            {
                var process = await _repository.GetProcessById(id);
                if (process == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var processDTO = _mapper.Map<ProcessDTO>(process);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = processDTO;

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

        public async Task<ServiceResponse<ProcessDTO>> AddProcess(ProcessDTO request)
        {
            ServiceResponse<ProcessDTO> _response = new();
            try
            {
                Process _newProcess = new Process()
                {
                    ProcessId = request.ProcessId,
                    //CustomerId = request.CustomerId,
                    Note = request.Note,
                    Status = "Processing",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                };

                if (!await _repository.AddProcess(_newProcess))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<ProcessDTO>(_newProcess);
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

        public async Task<ServiceResponse<ProcessDTO>> UpdateProcess(ProcessDTO request)
        {
            ServiceResponse<ProcessDTO> _response = new();
            try
            {
                var existingProcess = await _repository.GetProcessById(request.ProcessId);
                if (existingProcess == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingProcess.Note = request.Note;
                existingProcess.Status = request.Status;
                existingProcess.IsDeleted = request.isDelete;
                //existingProcess.ModifiedDate = DateTime.Now;
                //existingProcess.ModifiedBy = request.ModifiedBy;

                if (!await _repository.UpdateProcess(existingProcess))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _processDTO = _mapper.Map<ProcessDTO>(existingProcess);
                _response.Success = true;
                _response.Data = _processDTO;
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

        public async Task<ServiceResponse<ProcessDTO>> DeleteProcess(int id)
        {
            ServiceResponse<ProcessDTO> _response = new();
            try
            {
                var existingProcess = await _repository.GetProcessById(id);
                if (existingProcess == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (!await _repository.DeleteProcess(existingProcess))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _processDTO = _mapper.Map<ProcessDTO>(existingProcess);
                _response.Success = true;
                _response.Data = _processDTO;
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
