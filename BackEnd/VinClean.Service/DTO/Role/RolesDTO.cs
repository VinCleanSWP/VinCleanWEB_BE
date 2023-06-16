using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Roles
{
    public class RolesDTO
    {
        public int RoleId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Role Name cannot be less than 2 characters")]
        [MaxLength(50, ErrorMessage = "Role Name is too long")]
        public string Name { get; set; }
    }
}
