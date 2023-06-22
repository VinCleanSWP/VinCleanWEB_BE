using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Repository;
using VinClean.Service.DTO.Service;
using VinClean.Service.DTO;
using VinClean.Repo.Models;
namespace VinClean.Service.Service
{
    public interface ISvcService
    {
        Task<ServiceResponse<List<ServicesDTO>>> GetServiceList();
        Task<ServiceResponse<ServicesDTO>> GetServiceById(int id);
        Task<ServiceResponse<ServicesDTO>> AddService(ServicesDTO request);
        Task<ServiceResponse<ServicesDTO>> UpdateService(ServicesDTO request);
        Task<ServiceResponse<ServicesDTO>> DeleteService(int id);
    }

    public class SvcService : ISvcService
    {
        private readonly IServiceRepository _repository;
        private readonly IMapper _mapper;

        public SvcService(IServiceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<ServicesDTO>>> GetServiceList()
        {
            var services = await _repository.GetServiceList();
            var serviceDTOs = _mapper.Map<List<ServicesDTO>>(services);

            return new ServiceResponse<List<ServicesDTO>>
            {
                Success = true,
                Message = "OK",
                Data = serviceDTOs
            };
        }

        public async Task<ServiceResponse<ServicesDTO>> GetServiceById(int id)
        {
            var service = await _repository.GetServiceById(id);
            if (service == null)
            {
                return new ServiceResponse<ServicesDTO>
                {
                    Success = false,
                    Message = "NotFound"
                };
            }

            var serviceDTO = _mapper.Map<ServicesDTO>(service);

            return new ServiceResponse<ServicesDTO>
            {
                Success = true,
                Message = "OK",
                Data = serviceDTO
            };
        }

        public async Task<ServiceResponse<ServicesDTO>> AddService(ServicesDTO request)
        {
            var service = _mapper.Map<Repo.Models.Service>(request);

            if (await _repository.AddService(service))
            {
                var serviceDTO = _mapper.Map<ServicesDTO>(service);

                return new ServiceResponse<ServicesDTO>
                {
                    Success = true,
                    Message = "Created",
                    Data = serviceDTO
                };
            }

            return new ServiceResponse<ServicesDTO>
            {
                Success = false,
                Message = "RepoError"
            };
        }

        public async Task<ServiceResponse<ServicesDTO>> UpdateService(ServicesDTO request)
        {
            var existingService = await _repository.GetServiceById(request.ServiceId);
            if (existingService == null)
            {
                return new ServiceResponse<ServicesDTO>
                {
                    Success = false,
                    Message = "NotFound"
                };
            }

            _mapper.Map(request, existingService);

            if (await _repository.UpdateService(existingService))
            {
                var serviceDTO = _mapper.Map<ServicesDTO>(existingService);

                return new ServiceResponse<ServicesDTO>
                {
                    Success = true,
                    Message = "Updated",
                    Data = serviceDTO
                };
            }

            return new ServiceResponse<ServicesDTO>
            {
                Success = false,
                Message = "RepoError"
            };
        }

        public async Task<ServiceResponse<ServicesDTO>> DeleteService(int id)
        {
            var existingService = await _repository.GetServiceById(id);
            if (existingService == null)
            {
                return new ServiceResponse<ServicesDTO>
                {
                    Success = false,
                    Message = "NotFound"
                };
            }

            if (await _repository.DeleteService(existingService))
            {
                var serviceDTO = _mapper.Map<ServicesDTO>(existingService);

                return new ServiceResponse<ServicesDTO>
                {
                    Success = true,
                    Message = "Deleted",
                    Data = serviceDTO
                };
            }

            return new ServiceResponse<ServicesDTO>
            {
                Success = false,
                Message = "RepoError"
            };
        }
    }
}
