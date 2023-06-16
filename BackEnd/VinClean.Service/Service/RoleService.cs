using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Roles;

namespace VinClean.Service.Service
{
    public interface IRoleService
    {
        Task<ServiceResponse<List<RolesDTO>>> GetRoleList();
        Task<ServiceResponse<RolesDTO>> GetRoleById(int id);
        Task<ServiceResponse<RolesDTO>> AddRole(RolesDTO request);
        Task<ServiceResponse<RolesDTO>> UpdateRole(RolesDTO request);
        Task<ServiceResponse<RolesDTO>> DeleteRole(int id);
    }

    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<RolesDTO>>> GetRoleList()
        {
            var roles = await _repository.GetRoleList();
            var roleDTOs = _mapper.Map<List<RolesDTO>>(roles);

            return new ServiceResponse<List<RolesDTO>>
            {
                Success = true,
                Message = "OK",
                Data = roleDTOs
            };
        }

        public async Task<ServiceResponse<RolesDTO>> GetRoleById(int id)
        {
            var role = await _repository.GetRoleById(id);
            if (role == null)
            {
                return new ServiceResponse<RolesDTO>
                {
                    Success = false,
                    Message = "NotFound"
                };
            }

            var roleDTO = _mapper.Map<RolesDTO>(role);

            return new ServiceResponse<RolesDTO>
            {
                Success = true,
                Message = "OK",
                Data = roleDTO
            };
        }

        public async Task<ServiceResponse<RolesDTO>> AddRole(RolesDTO request)
        {
            var role = _mapper.Map<Role>(request);

            if (await _repository.AddRole(role))
            {
                var roleDTO = _mapper.Map<RolesDTO>(role);

                return new ServiceResponse<RolesDTO>
                {
                    Success = true,
                    Message = "Created",
                    Data = roleDTO
                };
            }

            return new ServiceResponse<RolesDTO>
            {
                Success = false,
                Message = "RepoError"
            };
        }

        public async Task<ServiceResponse<RolesDTO>> UpdateRole(RolesDTO request)
        {
            var existingRole = await _repository.GetRoleById(request.RoleId);
            if (existingRole == null)
            {
                return new ServiceResponse<RolesDTO>
                {
                    Success = false,
                    Message = "NotFound"
                };
            }

            _mapper.Map(request, existingRole);

            if (await _repository.UpdateRole(existingRole))
            {
                var roleDTO = _mapper.Map<RolesDTO>(existingRole);

                return new ServiceResponse<RolesDTO>
                {
                    Success = true,
                    Message = "Updated",
                    Data = roleDTO
                };
            }

            return new ServiceResponse<RolesDTO>
            {
                Success = false,
                Message = "RepoError"
            };
        }

        public async Task<ServiceResponse<RolesDTO>> DeleteRole(int id)
        {
            var existingRole = await _repository.GetRoleById(id);
            if (existingRole == null)
            {
                return new ServiceResponse<RolesDTO>
                {
                    Success = false,
                    Message = "NotFound"
                };
            }

            if (await _repository.DeleteRole(existingRole))
            {
                var roleDTO = _mapper.Map<RolesDTO>(existingRole);

                return new ServiceResponse<RolesDTO>
                {
                    Success = true,
                    Message = "Deleted",
                    Data = roleDTO
                };
            }

            return new ServiceResponse<RolesDTO>
            {
                Success = false,
                Message = "RepoError"
            };
        }
    }
}
