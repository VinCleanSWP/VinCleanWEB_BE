using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.WorkingBy;
using VinClean.Service.DTO.WorkingSlot;

namespace VinClean.Service.Service
{
    public interface IWorkingByService
    {
        Task<ServiceResponse<List<WorkingByDTO>>> GetWBList();
        Task<ServiceResponse<WorkingByDTO>> GetWBById(int id);
        Task<ServiceResponse<WorkingByDTO>> GetWBByProcessId(int id);
        Task<ServiceResponse<WorkingByDTO>> DeleteWB(int id);
        Task<ServiceResponse<WorkingByDTO>> AddWB(WorkingByDTO request);
        Task<ServiceResponse<WorkingByDTO>> UpdateWB(WorkingByDTO request);
        Task<ServiceResponse<LocationDTO>> UpdateLocation(LocationDTO request);
        Task<ServiceResponse<WorkingByDTO>> AcceptRequest(WorkingByDTO request);
    }
    public class WorkingByService : IWorkingByService
    {
        private readonly IWorkingByRepository _repository;
        private readonly IProcessSlotRepository _PRrepository;
        private readonly IMapper _mapper;
        public WorkingByService(IWorkingByRepository repository, IMapper mapper, IProcessSlotRepository pRrepository)
        {
            _repository = repository;
            _mapper = mapper;
            _PRrepository = pRrepository;
        }

        public async Task<ServiceResponse<List<WorkingByDTO>>> GetWBList()
        {
            ServiceResponse<List<WorkingByDTO>> _response = new();
            try
            {
                var ListWslot = await _repository.GetWorkingByList();
                var ListWslotDTO = new List<WorkingByDTO>();
                foreach (var WSlot in ListWslot)
                {
                    ListWslotDTO.Add(_mapper.Map<WorkingByDTO>(WSlot));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListWslotDTO;
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

        public async Task<ServiceResponse<WorkingByDTO>> GetWBById(int id)
        {
            ServiceResponse<WorkingByDTO> _response = new();
            try
            {
                var WSlot = await _repository.GetWorkingByById(id);
                if (WSlot == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var WSlotDTO = _mapper.Map<WorkingByDTO>(WSlot);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = WSlotDTO;

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
        public async Task<ServiceResponse<WorkingByDTO>> GetWBByProcessId(int id)
        {
            ServiceResponse<WorkingByDTO> _response = new();
            try
            {
                var WSlot = await _repository.GetWorkingByByProcessId(id);
                if (WSlot == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var WSlotDTO = _mapper.Map<WorkingByDTO>(WSlot);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = WSlotDTO;

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

        public async Task<ServiceResponse<WorkingByDTO>> UpdateWB(WorkingByDTO request)
        {
            ServiceResponse<WorkingByDTO> _response = new();
            try
            {
                var existingWSlot = await _repository.GetWorkingByById(request.EmployeeId);
                if (existingWSlot == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                existingWSlot.EmployeeId = request.EmployeeId;

                if (!await _repository.UpdateWorkingBy(existingWSlot))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _WSlotDTO = _mapper.Map<WorkingByDTO>(existingWSlot);
                _response.Success = true;
                _response.Data = _WSlotDTO;
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


        public async Task<ServiceResponse<LocationDTO>> UpdateLocation(LocationDTO request)
        {
            ServiceResponse<LocationDTO> _response = new();
            //try
            //{
                var existingWSlot = await _repository.GetWorkingByByProcessId(request.ProcessId);
                if (existingWSlot == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                existingWSlot.Latitude = request.Latitude;
                existingWSlot.Longtitude = request.Longtitude;
                if (!await _repository.UpdateWorkingBy(existingWSlot))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _WSlotDTO = _mapper.Map<LocationDTO>(existingWSlot);
                _response.Success = true;
                _response.Data = _WSlotDTO;
                _response.Message = "Updated";

            //}
            //catch (Exception ex)
            //{
            //    _response.Success = false;
            //    _response.Data = null;
            //    _response.Message = "Error";
            //    _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            //}
            return _response;
        }


        public async Task<ServiceResponse<WorkingByDTO>> AcceptRequest(WorkingByDTO request)
        {
            ServiceResponse<WorkingByDTO> _response = new();
            try
            {
                var existingWSlot = await _repository.GetWorkingByByProcessId(request.ProcessId);
                if (existingWSlot == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                existingWSlot.EmployeeId = request.EmployeeId;

                var check1 = await _repository.UpdateWorkingBy(existingWSlot);

                //Update ProcessRequest
                var existingProcess = await _PRrepository.GetPSById(request.ProcessId);
                if (existingProcess == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                existingProcess.Satus = "Accepted";
                existingProcess.NewEmployeeId = request.EmployeeId;
                var check2 = await _PRrepository.UpdatePS(existingProcess);
                //Update ProcessRequest

                if (!check1 && !check2)
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _WSlotDTO = _mapper.Map<WorkingByDTO>(existingWSlot);
                _response.Success = true;
                _response.Data = _WSlotDTO;
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

        public async Task<ServiceResponse<WorkingByDTO>> AddWB(WorkingByDTO request)
        {
            ServiceResponse<WorkingByDTO> _response = new();
            try
            {
                WorkingBy _newWB = new WorkingBy()
                {
                    ProcessId = request.ProcessId,
                    EmployeeId = request.EmployeeId,

                };

                if (!await _repository.AddWorkingBy(_newWB))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<WorkingByDTO>(_newWB);
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

        public async Task<ServiceResponse<WorkingByDTO>> DeleteWB(int id)

        {
            ServiceResponse<WorkingByDTO> _response = new();
            try
            {
                var existingWB = await _repository.GetWorkingByByProcessId(id);
                if (existingWB == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }


                if (!await _repository.DeleteWorkingBy(existingWB))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    return _response;
                }

                _response.Success = true;
                _response.Message = "SoftDeleted";

            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message)
    };
            }
            return _response;
        }
    }
}
