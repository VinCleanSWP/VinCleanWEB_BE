using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO
{
    public class AccountDTO
    {
        public int AccountId { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public int RoleId { get; set; }

        public string Status { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
