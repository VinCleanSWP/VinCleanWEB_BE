using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.ServiceManage;

namespace VinClean.Service.Service
{
    public interface ISvcManageService
    {
        Task<List<SvcManageDTO>> GetServiceManages();
        Task<SvcManageDTO> GetServiceManageById(int employeeId, int serviceId);
        Task<ServiceResponse<SvcManageDTO>> AddServiceManage(SvcManageDTO serviceManageDTO);
        Task<ServiceResponse<SvcManageDTO>> UpdateServiceManage(SvcManageDTO serviceManageDTO);
        Task<ServiceResponse<SvcManageDTO>> DeleteServiceManage(int employeeId, int serviceId);
    }

    public class SvcManageService : ISvcManageService
    {
        private readonly IServiceManageRepository _repository;
        private readonly IMapper _mapper;

        public SvcManageService(IServiceManageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SvcManageDTO>> GetServiceManages()
        {
            var serviceManages = await _repository.GetServiceManages();
            return _mapper.Map<List<SvcManageDTO>>(serviceManages);
        }

        public async Task<SvcManageDTO> GetServiceManageById(int employeeId, int serviceId)
        {
            var serviceManage = await _repository.GetServiceManageById(employeeId, serviceId);
            return _mapper.Map<SvcManageDTO>(serviceManage);
        }

        public async Task<ServiceResponse<SvcManageDTO>> AddServiceManage(SvcManageDTO serviceManageDTO)
        {
            var serviceManage = _mapper.Map<ServiceManage>(serviceManageDTO);

            if (await _repository.GetServiceManageById(serviceManage.EmployeeId.Value, serviceManage.ServiceId.Value) != null)
            {
                return new ServiceResponse<SvcManageDTO>
                {
                    Success = false,
                    Message = "ServiceManage already exists",
                    Data = null
                };
            }

            if (await _repository.AddServiceManage(serviceManage))
            {
                return new ServiceResponse<SvcManageDTO>
                {
                    Success = true,
                    Message = "ServiceManage added successfully",
                    Data = _mapper.Map<SvcManageDTO>(serviceManage)
                };
            }

            return new ServiceResponse<SvcManageDTO>
            {
                Success = false,
                Message = "Failed to add ServiceManage",
                Data = null
            };
        }

        public async Task<ServiceResponse<SvcManageDTO>> UpdateServiceManage(SvcManageDTO serviceManageDTO)
        {
            var existingServiceManage = await _repository.GetServiceManageById(serviceManageDTO.EmployeeId, serviceManageDTO.ServiceId);

            if (existingServiceManage == null)
            {
                return new ServiceResponse<SvcManageDTO>
                {
                    Success = false,
                    Message = "ServiceManage not found",
                    Data = null
                };
            }

            existingServiceManage.StartDate = serviceManageDTO.StartDate;
            existingServiceManage.EndDate = serviceManageDTO.EndDate;

            if (await _repository.UpdateServiceManage(existingServiceManage))
            {
                return new ServiceResponse<SvcManageDTO>
                {
                    Success = true,
                    Message = "ServiceManage updated successfully",
                    Data = _mapper.Map<SvcManageDTO>(existingServiceManage)
                };
            }

            return new ServiceResponse<SvcManageDTO>
            {
                Success = false,
                Message = "Failed to update ServiceManage",
                Data = null
            };
        }

        public async Task<ServiceResponse<SvcManageDTO>> DeleteServiceManage(int employeeId, int serviceId)
        {
            var existingServiceManage = await _repository.GetServiceManageById(employeeId, serviceId);

            if (existingServiceManage == null)
            {
                return new ServiceResponse<SvcManageDTO>
                {
                    Success = false,
                    Message = "ServiceManage not found",
                    Data = null
                };
            }

            if (await _repository.DeleteServiceManage(existingServiceManage))
            {
                return new ServiceResponse<SvcManageDTO>
                {
                    Success = true,
                    Message = "ServiceManage deleted successfully",
                    Data = _mapper.Map<SvcManageDTO>(existingServiceManage)
                };
            }

            return new ServiceResponse<SvcManageDTO>
            {
                Success = false,
                Message = "Failed to delete ServiceManage",
                Data = null
            };
        }
    }
}
