using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
        public interface IAccountRepository
        {
            Task<ICollection<Account>> GetAccountList();
            Task<Account> GetAccountById(int id);
            Task<bool> AddAccount(Account account);
            Task<bool>DeleteAccount(Account account);
            Task<bool>UpdateAccount(Account account);
            Task<bool> CheckEmailAccountExist(String email);
    }

        public class AccountRepository : IAccountRepository
        {
            private readonly ServiceAppContext _context;
            public AccountRepository(ServiceAppContext context)
            {
                _context = context;
            }

        async Task<ICollection<Account>> IAccountRepository.GetAccountList()
        {
            return await _context.Accounts.ToListAsync();
        }
        async Task<Account> IAccountRepository.GetAccountById(int id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == id);
        }

        async Task<bool> IAccountRepository.AddAccount(Account account)
        {
             _context.Accounts.Add(account);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IAccountRepository.DeleteAccount(Account account)
        {
            _context.Accounts.Remove(account);
            return await _context.SaveChangesAsync() > 0 ? true : false;
            
        }
        async Task<bool> IAccountRepository.UpdateAccount(Account account)
        {
            _context.Accounts.Update(account);
            return await _context.SaveChangesAsync() > 0 ? true : false;

        }
        async Task<bool> IAccountRepository.CheckEmailAccountExist(string email)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
        }
    }

}
