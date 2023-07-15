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
        Task<ServiceResponse<ProcessDTO>> UpdateStartWorking(ProcessStartWorking process);
        Task<ServiceResponse<ProcessDTO>> UpdateEndWorking(ProcessEndWorking process);
        Task<ServiceResponse<ProcessDTO>> UpdateStatusCompleted(int id);
        Task<ServiceResponse<ProcessDTO>> DeleteProcess(int id);
    }

    public class ProcessService : IProcessService
    {
        private readonly IProcessDetailRepository _PDrepository;
        private readonly IServiceRepository _serviceRepo;
        private readonly IProcessRepository _repository;
        private readonly ICustomerRepository _Curepository;
        private readonly IWorkingByRepository _WBrepository;
        private readonly IProcessImageRepository _PImgrepository;
        public readonly IMapper _mapper;
        public ProcessService(IProcessRepository repository, IMapper mapper, IProcessDetailRepository pDrepository, 
            IServiceRepository serviceRepo, ICustomerRepository Curepository, IWorkingByRepository WBrepository, IProcessImageRepository pImgrepository)
        {
            _repository = repository;
            _mapper = mapper;
            _PDrepository = pDrepository;
            _serviceRepo = serviceRepo;
            _Curepository = Curepository;
            _WBrepository = WBrepository;
            _PImgrepository = pImgrepository;
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
            var customer = await _Curepository.GetCustomerById(request.CustomerId);

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
                Price = request.Price,
                PointUsed = request.PointUsed,
                IsDeleted = false,
            };
            var check1 = await _repository.AddProcess(_newProcess);

                ProcessDetail _processDetail = new ProcessDetail()
                {
                    ProcessId = _newProcess.ProcessId,
                    ServiceId = request.ServiceId,
                    
                };
                var check2 = await _PDrepository.AddPD(_processDetail);

                //Update TotalPoint in Cutomer
                customer.TotalPoint = customer.TotalPoint - request.PointUsed;
                var check3 = await _Curepository.UpdateCustomer(customer);

            ProcessImage _processImage1 = new ProcessImage()
            {
                 ProcessId = _newProcess.ProcessId,
                 Type = "Verify",
                 Name = "Start Working"
            };
            await _PImgrepository.AddProcessImage(_processImage1);
            ProcessImage _processImage2 = new ProcessImage()
            {
                 ProcessId = _newProcess.ProcessId,
                 Type = "Processing",
                 Name = "Processing"
            };
            await _PImgrepository.AddProcessImage(_processImage2);
            ProcessImage _processImage3 = new ProcessImage()
            {
                ProcessId = _newProcess.ProcessId,
                Type = "Completed",
                Name = "End Working"
            };
            await _PImgrepository.AddProcessImage(_processImage3);


            if (!check1&&!check2&&!check3)
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
                ProcessId = _newProcess.ProcessId,
                ServiceId = request.ServiceId,

            };
            var check2 = await _PDrepository.AddPD(_processDetail);


            if (!check1 && !check2)
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

        public async Task<ServiceResponse<ProcessDTO>> UpdateStartWorking(ProcessStartWorking request)
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

        public async Task<ServiceResponse<ProcessDTO>> UpdateSubPrice(UpdateSubPirce request)
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

                existingProcess.SubPrice = request.SubPrice;

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

        public async Task<ServiceResponse<ProcessDTO>> UpdateEndWorking(ProcessEndWorking request)
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


        public async Task<ServiceResponse<ProcessDTO>> UpdateStatusCompleted(int id)
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

                existingProcess.Status = "Completed";

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
            /*try
            {*/
                var existingProcess = await _repository.GetProcessById(id);
                var existingProcessPD = await _PDrepository.GetPDById(id);
                var existingWorkingBy = await _WBrepository.GetWorkingByByProcessId(id);
                var existingProcessImg = await _PImgrepository.ProcessImageListByProcessId(id);
                if (existingProcess == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                foreach ( var img in existingProcessImg)
                {
                    await _PImgrepository.DeleteProcessImage(img);
                }

                if (!await _repository.DeleteProcess(existingProcess) 
                && (!await _PDrepository.DeletePD(existingProcessPD))
                && (!await _WBrepository.DeleteWorkingBy(existingWorkingBy)))
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

            /*}
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }*/
            return _response;
        }
    }
}
