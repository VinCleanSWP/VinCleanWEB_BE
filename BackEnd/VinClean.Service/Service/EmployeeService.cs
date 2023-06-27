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
using VinClean.Service.DTO.Order;

namespace VinClean.Service.Service
{
    public interface IEmployeeService
    {
        Task<ServiceResponse<List<EmployeeDTO>>> GetEmployeeList();
        Task<ServiceResponse<List<EmployeeDTO>>> SearchEmployee(string search);
        Task<ServiceResponse<EmployeeDTO>> GetEmployeeById(int id);
        Task<ServiceResponse<EmployeeDTO>> AddEmployee(EmployeeDTO request);
        Task<ServiceResponse<EmployeeDTO>> UpdateEmployee(EmployeeDTO request);
        Task<ServiceResponse<EmployeeDTO>> DeleteEmployee(int id);

    }
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<EmployeeDTO>> AddEmployee(EmployeeDTO request)
        {
            ServiceResponse<EmployeeDTO> _response = new();
            try
            {

                Employee _newEmployee = new Employee()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Phone = request.Phone,
                    Status = "Active",
                    
                };

                if (!await _repository.AddEmployee(_newEmployee))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<EmployeeDTO>(_newEmployee);
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

        public async Task<ServiceResponse<EmployeeDTO>> DeleteEmployee(int id)
        {
            ServiceResponse<EmployeeDTO> _response = new();
            try
            {
                var existingEmployee = await _repository.GetEmployeeById(id);
                if (existingEmployee == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if (await _repository.DeleteEmployee(existingEmployee))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _OrderDTO = _mapper.Map<EmployeeDTO>(existingEmployee);
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

        public async Task<ServiceResponse<EmployeeDTO>> GetEmployeeById(int id)
        {
            ServiceResponse<EmployeeDTO> _response = new();
            try
            {
                var employee = await _repository.GetEmployeeById(id);
                if (employee == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var employeedto = _mapper.Map<EmployeeDTO>(employee);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = employeedto;

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

        public async Task<ServiceResponse<List<EmployeeDTO>>> GetEmployeeList()
        {
            ServiceResponse<List<EmployeeDTO>> _response = new();
            try
            {
                var ListEmployee = await _repository.GetEmployeeList();
                var ListEmployeeDTO = new List<EmployeeDTO>();
                foreach (var employee in ListEmployee)
                {
                    ListEmployeeDTO.Add(_mapper.Map<EmployeeDTO>(employee));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListEmployeeDTO;
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

        public async Task<ServiceResponse<List<EmployeeDTO>>> SearchEmployee(string search)
        {
            ServiceResponse<List<EmployeeDTO>> _response = new();
            try
            {
                var ListEmployee = await _repository.SearchEmployee(search);
                var ListEmployeeDTO = new List<EmployeeDTO>();
                foreach (var employee in ListEmployee)
                {
                    ListEmployeeDTO.Add(_mapper.Map<EmployeeDTO>(employee));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListEmployeeDTO;
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

        public async Task<ServiceResponse<EmployeeDTO>> UpdateEmployee(EmployeeDTO request)
        {
            ServiceResponse<EmployeeDTO> _response = new();
            try
            {
                var existingEmployee = await _repository.GetEmployeeById(request.EmployeeId);
                if (existingEmployee == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }
                // cac gia trị cho sua
                existingEmployee.FirstName = request.FirstName;
                existingEmployee.LastName = request.LastName;
                existingEmployee.Phone = request.Phone;
                existingEmployee.Status = request.Status;


                if (!await _repository.UpdateEmployee(existingEmployee))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _EmployeeDTO = _mapper.Map<EmployeeDTO>(existingEmployee);
                _response.Success = true;
                _response.Data = _EmployeeDTO;
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
