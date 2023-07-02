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
using static System.Net.Mime.MediaTypeNames;

namespace VinClean.Service.Service
{
    public interface IEmployeeService
    {
        Task<ServiceResponse<List<EmployeeDTO>>> GetEmployeeList();
        Task<ServiceResponse<List<EmployeeDTO>>> SearchEmployee(string search);
        Task<ServiceResponse<EmployeeDTO>> GetEmployeeById(int id);
        Task<ServiceResponse<List<EmployeeDTO>>> SelectEmployeeList(String startTime, String endTime, String date);
        Task<ServiceResponse<EmployeeDTO>> AddEmployee(RegisterEmployeeDTO request);
        Task<ServiceResponse<EmployeeDTO>> UpdateEmployee(UpdateEmployeeDTO request);
        Task<ServiceResponse<EmployeeDTO>> DeleteEmployee(int id);

    }
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;

        public EmployeeService(IEmployeeRepository repository, IAccountRepository accountRepository, IMapper mapper)
        {
            _repository = repository;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<EmployeeDTO>> AddEmployee(RegisterEmployeeDTO request)
        {
            ServiceResponse<EmployeeDTO> _response = new();
            //try
            //{
                var _newAccount = new Account()
                {
                    Name = request.Name,
                    Password = request.Password,
                    Email = request.Email,
                    RoleId = 2, // assign a default role for new accounts
                    Status = "Active", // set the status to active by default
                    IsDeleted = false, // set the isDeleted flag to false by default
                    CreatedDate = DateTime.Now, // set the created date to the current date/time
                    Gender = request.Gender,
                    Img = request.Img,


                };
                await _accountRepository.AddAccount(_newAccount);

                Employee _newEmployee = new Employee()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    StartDate = null,
                    EndDate = null,
                    AccountId = _newAccount.AccountId,
                    Phone = request.Phone,
                    Status = "Available",
                    
                };

                if (!await _repository.AddEmployee(_newEmployee))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<EmployeeDTO>(await _repository.GetEmployeeById(_newEmployee.EmployeeId));
                _response.Message = "Created";

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

        public async Task<ServiceResponse<List<EmployeeDTO>>> SelectEmployeeList(String startTime, String endTime, String date)
        {
            ServiceResponse<List<EmployeeDTO>> _response = new();
            //try
            //{
                var ListEmployee = await _repository.SelectEmployeeList(startTime,endTime,date);
                var ListEmployeeDTO = new List<EmployeeDTO>();
                foreach (var employee in ListEmployee)
                {
                    ListEmployeeDTO.Add(_mapper.Map<EmployeeDTO>(employee));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListEmployeeDTO;
            //}
            //catch (Exception ex)
            //{
            //    _response.Success = false;
            //    _response.Message = "serive";
            //    _response.Data = null;
            //    _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            //}
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

        public async Task<ServiceResponse<EmployeeDTO>> UpdateEmployee(UpdateEmployeeDTO request)
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
                    var _newAccount = await _accountRepository.GetAccountById(existingEmployee.Account.AccountId);
                    _newAccount.Name = request.Name;
                    _newAccount.Password = request.Password;
                    _newAccount.Email = request.Email;
                    _newAccount.Gender = request.Gender;
                    _newAccount.Img = request.Img;

                    await _accountRepository.UpdateAccount(_newAccount);
                
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
