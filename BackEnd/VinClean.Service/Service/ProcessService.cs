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
using VinClean.Service.DTO.Process;
using VinClean.Service.DTO.WorkingSlot;
using VinClean.Service.DTO.WorkingBy;
using VinClean.Service.DTO.Employee;

// Pass data from Repo to Controller

namespace VinClean.Service.Service
{
    public interface IProcessService
    {
        Task<ServiceResponse<List<ProcessModeDTO>>> GetProcessList();
        Task<ServiceResponse<ProcessDTO>> GetProcessById(int id);
        Task<ServiceResponse<ProcessModeDTO>> GetAllInfoById(int id);
        Task<ServiceResponse<ProcessDTO>> AddProcess(NewBooking process);
        Task<ServiceResponse<ProcessDTO>> UpdateProcess(ProcessDTO process);
        Task<ServiceResponse<ProcessDTO>> UpdateStartWorking(ProcessDTO process);
        Task<ServiceResponse<ProcessDTO>> UpdateEndWorking(ProcessDTO process);
        Task<ServiceResponse<ProcessDTO>> UpdateStatus(ProcessDTO process);
        Task<ServiceResponse<ProcessDTO>> DeleteProcess(int id);
    }

    public class ProcessService : IProcessService
    {
        private readonly IProcessDetailRepository _PDrepository;
        private readonly IServiceRepository _serviceRepo;
        private readonly IProcessRepository _repository;
        public readonly IMapper _mapper;
        public ProcessService(IProcessRepository repository, IMapper mapper, IProcessDetailRepository pDrepository, IServiceRepository serviceRepo)
        {
            _repository = repository;
            _mapper = mapper;
            _PDrepository = pDrepository;  
            _serviceRepo = serviceRepo;
        }

        public async Task<ServiceResponse<List<ProcessModeDTO>>> GetProcessList()
        {
            ServiceResponse<List<ProcessModeDTO>> _response = new();
            /*try
            {*/
                var listProcess = await _repository.GetProcesslist();
                var listProcessDTO = new List<ProcessModeDTO>();
                foreach (var process in listProcess)
                {
                    listProcessDTO.Add(_mapper.Map<ProcessModeDTO>(process));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = listProcessDTO;
           /* }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }*/
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

        public async Task<ServiceResponse<ProcessModeDTO>> GetAllInfoById(int id)
        {
            ServiceResponse<ProcessModeDTO> _response = new();
            try
            {
                var process = await _repository.GetAllInfoById(id);
               /* var process_dto = _mapper.Map<ProcessInfo>(process);*/
                if (process == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                /*var processDTO = _mapper.Map<ProcessModeDTO>(process);*/
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = process;

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

        public async Task<ServiceResponse<ProcessDTO>> AddProcess(NewBooking request)
        {
            ServiceResponse<ProcessDTO> _response = new();
            /*            try
                        {*/
            var service = await _serviceRepo.GetServiceById(request.ServiceId);

            Process _newProcess = new Process()
            {
                CustomerId = request.CustomerId,
                Note = request.Note,
                Status = "Incoming",
                StarTime = request.StarTime,
                EndTime = request.StarTime + TimeSpan.FromHours((int)service.MinimalSlot), 
                CreatedDate = DateTime.Now,
                Date = request.Date,
                Phone = request.Phone,
                Address = request.Address,
                IsDeleted = false,
                };
                var check1 = await _repository.AddProcess(_newProcess);

                ProcessDetail _processDetail = new ProcessDetail()
                {
                    ProcessId = _newProcess.ProcessId,
                    ServiceId = request.ServiceId,
                    
                };
                var check2 = await _PDrepository.AddPD(_processDetail);


                if (!check1&&!check2)
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<ProcessDTO>(_newProcess);
                _response.Message = "Created";

/*            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }*/

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
                existingProcess.Date = request.Date;
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

        public async Task<ServiceResponse<ProcessDTO>> UpdateStartWorking(ProcessDTO request)
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

                existingProcess.StartWorking = request.StartWorking;
                existingProcess.Status = "Processing";

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

        public async Task<ServiceResponse<ProcessDTO>> UpdateEndWorking(ProcessDTO request)
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

                existingProcess.EndWorking = request.EndWorking;

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


        public async Task<ServiceResponse<ProcessDTO>> UpdateStatus(ProcessDTO request)
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

                existingProcess.Status = request.Status;

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
