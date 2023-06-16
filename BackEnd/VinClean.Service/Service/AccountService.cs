using AutoMapper;
using Azure;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;

namespace VinClean.Service.Service
{
    public interface IAccountService
    {
        Task<ServiceResponse<List<AccountdDTO>>> GetAccountList();
        Task<ServiceResponse<AccountdDTO>> GetAccountById(int id);
        Task<ServiceResponse<AccountdDTO>> AddAccount(AccountdDTO request);
        Task<ServiceResponse<AccountdDTO>> UpdateAccount(AccountdDTO request);
        Task<ServiceResponse<AccountdDTO>> DeleteAccount(int id);

    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly IMapper    _mapper;
        public AccountService (IAccountRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            
        }

        public async Task<ServiceResponse<List<AccountdDTO>>> GetAccountList()
        {
            ServiceResponse<List<AccountdDTO>> _response = new();
            try
            {
                var ListAccount = await _repository.GetAccountList();
                var ListAccountDTO = new List<AccountdDTO>();
                foreach (var account in ListAccount)
                {
                    ListAccountDTO.Add(_mapper.Map<AccountdDTO>(account));
                }
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = ListAccountDTO;
            }catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;

        }

        public async Task<ServiceResponse<AccountdDTO>> GetAccountById(int id)
        {
            ServiceResponse<AccountdDTO> _response = new();
            try
            {
                var account = await _repository.GetAccountById(id);
                if(account == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var accoundto = _mapper.Map<AccountdDTO>(account);
                _response.Success = true;
                _response.Message = "OK";
                _response.Data = accoundto;

            }
            catch(Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
                
        }

        public async Task<ServiceResponse<AccountdDTO>> AddAccount(AccountdDTO request)
        {
            ServiceResponse<AccountdDTO> _response = new();
            try
            {
                if (await _repository.CheckEmailAccountExist(request.Email))
                {
                    _response.Message = "Exist";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }
                Account _newAccount = new Account()
                {
                    Email = request.Email,
                    Password = request.Password,
                    Name = request.Name,
                    RoleId = request.RoleId,
                    Status = request.Status,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,

                };

                if (!await _repository.AddAccount(_newAccount))
                {
                    _response.Error = "RepoError";
                    _response.Success = false;
                    _response.Data = null;
                    return _response;
                }

                _response.Success = true;
                _response.Data = _mapper.Map<AccountdDTO>(_newAccount);
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
        public async Task<ServiceResponse<AccountdDTO>> UpdateAccount(AccountdDTO request)
        {
            ServiceResponse<AccountdDTO> _response = new();
            try
            {
                var existingAccount = await _repository.GetAccountById(request.AccountId);
                if(existingAccount == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                existingAccount.Email = request.Email;
                existingAccount.Password = request.Password;
                existingAccount.Name = request.Name;
                existingAccount.Status = request.Status;
                existingAccount.IsDeleted = request.IsDeleted;

                if(!await _repository.UpdateAccount(existingAccount))
                {
                    _response.Success = false;
                    _response.Message= "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _accountDTO = _mapper.Map<AccountdDTO>(existingAccount);
                _response.Success = true;
                _response.Data = _accountDTO;
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

        public async Task<ServiceResponse<AccountdDTO>> DeleteAccount(int id)
        {
            ServiceResponse<AccountdDTO> _response = new();
            try
            {
                var existingAccount = await _repository.GetAccountById(id);
                if (existingAccount == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    _response.Data = null;
                    return _response;
                }

                if(await _repository.DeleteAccount(existingAccount))
                {
                    _response.Success = false;
                    _response.Message = "RepoError";
                    _response.Data = null;
                    return _response;
                }

                var _accountDTO = _mapper.Map<AccountdDTO>(existingAccount);
                _response.Success = true;
                _response.Data = _accountDTO;
                _response.Message = "Deleted";

            }
            catch(Exception ex)
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
