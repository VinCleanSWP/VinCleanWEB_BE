using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Repo.Repository
{
    public interface IRoleRepository
    {
        Task<ICollection<Role>> GetRoleList();
        Task<Role> GetRoleById(int id);
        Task<bool> AddRole(Role role);
        Task<bool> DeleteRole(Role role);
        Task<bool> UpdateRole(Role role);
    }

    public class RoleRepository : IRoleRepository
    {
        private readonly ServiceAppContext _context;

        public RoleRepository(ServiceAppContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Role>> GetRoleList()
        {
            return await _context.Roles.ToListAsync();
        }


        public async Task<Role> GetRoleById(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);
        }


        public async Task<bool> AddRole(Role role)
        {
            _context.Roles.Add(role);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteRole(Role role)
        {
            _context.Roles.Remove(role);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
