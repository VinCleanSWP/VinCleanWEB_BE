﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.DTO.CustomerResponse;

namespace VinClean.Service.Service
{
    public interface ICustomerService
    {
        Task<ServiceResponse<List<CustomerDTO>>> GetCustomerList();
        Task<ServiceResponse<CustomerDTO>> GetCustomerById(int id);
        Task<ServiceResponse<CustomerDTO>> Register(RegisterDTO request);
        Task<ServiceResponse<CustomerDTO>> UpdateCustomer(UpdateDTO request);

/*        Task<ServiceResponse<RegisterDTO>> DeleteCustomer(int id);*/
    }
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        public CustomerService(ICustomerRepository customertRepository, IAccountRepository accountRepository, IMapper mapper ) 
        { 
            _accountRepository = accountRepository;
            _customerRepository = customertRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<CustomerDTO>>> GetCustomerList()
        {
            ServiceResponse<List<CustomerDTO>> _response = new();
            try
            {
                var ListCustomer = await _customerRepository.GetCustomerList();
                var ListCustomerDTO = new List<CustomerDTO>();
                foreach (var customer in ListCustomer)
                {
/*                    var account = await _accountRepository.GetAccountById((int)customer.AccountId);*/ // Get the account for the customer
                    var customerDTO = _mapper.Map<CustomerDTO>(customer);
/*                    customerDTO.Account = _mapper.Map<AccountdDTO>(account);*/ // Map the account information to the DTO
                    ListCustomerDTO.Add(customerDTO);

                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListCustomerDTO;
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

        public async Task<ServiceResponse<CustomerDTO>> GetCustomerById(int id)
        {
            ServiceResponse<CustomerDTO> _response = new();
            try
            {
                var customer = await _customerRepository.GetCustomerById(id);
                if (customer == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = _mapper.Map<CustomerDTO>(customer);


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

        public async Task<ServiceResponse<CustomerDTO>> Register(RegisterDTO request)
        {
            ServiceResponse<CustomerDTO> _response = new();
            try
            {
                if (await _customerRepository.CheckEmailCustomerExist(request.Email))
                {
                    _response.Message = "Exist";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }
                var _newAccount = new Account
                {
                    Name = request.FirstName + " " + request.LastName,
                    Password = request.Password,
                    Email = request.Email,
                    RoleId = 1, // assign a default role for new accounts
                    Status = "Active", // set the status to active by default
                    IsDeleted = false, // set the isDeleted flag to false by default
                    CreatedDate = DateTime.Now // set the created date to the current date/time
                };
                await _accountRepository.AddAccount(_newAccount);

                var _newcustomer = new Customer
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Phone = request.Phone,
                    Address = request.Address,
                    AccountId = _newAccount.AccountId,
                    Status = "Active" // set the status to active by default
                };
                if (!await _customerRepository.AddCustomer(_newcustomer))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }   
                _response.Success = true;
                _response.Data = _mapper.Map<CustomerDTO>(await _customerRepository.GetCustomerById(_newcustomer.CustomerId));
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

        public async Task<ServiceResponse<CustomerDTO>> UpdateCustomer(UpdateDTO request)
        {
            ServiceResponse<CustomerDTO> _response = new();
            try
            {
                var existingCustomer = await _customerRepository.GetCustomerById(request.CustomerId);
                if (existingCustomer == null)
                {
                    _response.Message = "NotFound";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }
                var _newAccount = await _accountRepository.GetAccountById(existingCustomer.Account.AccountId);
                _newAccount.Name = request.FirstName + " " + request.LastName;
                _newAccount.Password = request.Password;
                _newAccount.Email = request.Email;
                await _accountRepository.UpdateAccount(_newAccount);

                existingCustomer.FirstName = request.FirstName;
                existingCustomer.LastName = request.LastName;
                existingCustomer.Phone = request.Phone;
                existingCustomer.Address = request.Address;

                if (!await _customerRepository.UpdateCustomer(existingCustomer))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<CustomerDTO>(await _customerRepository.GetCustomerById(existingCustomer.CustomerId));
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
       
    }
}
