using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

using VinClean.Service.DTO.Account;
using VinClean.Service.DTO.Blog;
using VinClean.Service.DTO.Category;
using VinClean.Service.DTO.Comment;
using VinClean.Service.DTO.CustomerResponse;

using VinClean.Service.DTO.Employee;
using VinClean.Service.DTO.Order;
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
using VinClean.Service.DTO.Process;
using VinClean.Service.DTO.Rating;
using VinClean.Service.DTO.Role;
using VinClean.Service.DTO.Service;
using VinClean.Service.DTO.ServiceManage;

namespace VinClean.Service.DTO
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {

            CreateMap<VinClean.Repo.Models.Account, AccountdDTO>().ReverseMap();
            CreateMap<VinClean.Repo.Models.Account, LoginDTO>().ReverseMap();
            CreateMap<Customer, RegisterDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();

            CreateMap<VinClean.Repo.Models.Employee, EmployeeDTO>().ReverseMap();
            CreateMap<VinClean.Repo.Models.Order, OrderDTO>().ReverseMap();
            CreateMap<OrderDetail,OrderDetailDTO>().ReverseMap();
            CreateMap<VinClean.Repo.Models.FinshedBy, FinishedByDTO>().ReverseMap();

            CreateMap<Repo.Models.Blog, BlogDTO>().ReverseMap();
            CreateMap<Repo.Models.Comment, CommentDTO>().ReverseMap();
            CreateMap<Repo.Models.Category, CategoryDTO>().ReverseMap();
<<<<<<< Updated upstream

            CreateMap<Role, RolesDTO>().ReverseMap();
            CreateMap<Repo.Models.Service, ServicesDTO>().ReverseMap();
            CreateMap<Repo.Models.ServiceManage, SvcManageDTO>().ReverseMap();
=======
           
           CreateMap<VinClean.Repo.Models.Employee, EmployeeProfileDTO>().ReverseMap();
            CreateMap<VinClean.Repo.Models.Employee, ModifyEmployeeProfileDTO>().ReverseMap();
            CreateMap<Customer, CustomerProfileDTO>().ReverseMap();
            CreateMap<Customer, ModifyCustomerProfileDTO>().ReverseMap();
            
            CreateMap<Repo.Models.Role, RoleDTO>().ReverseMap();
            CreateMap<Repo.Models.Service, ServiceDTO>().ReverseMap();
            CreateMap<Repo.Models.ServiceManage, ServiceManageDTO>().ReverseMap();
>>>>>>> Stashed changes

            CreateMap<Repo.Models.Process, ProcessDTO>().ReverseMap();
            CreateMap<ProcessDetail, ProcessDetailDTO>().ReverseMap();
            CreateMap<ProcessSlot, ProcessSlotDTO>().ReverseMap();
<<<<<<< Updated upstream

<<<<<<< Updated upstream
=======
>>>>>>> Stashed changes
=======
            CreateMap<Repo.Models.Rating, RatingDTO>().ReverseMap();
            CreateMap<Repo.Models.Service, RateServiceDTO>().ReverseMap();
            CreateMap<Customer, RateByDTO>().ReverseMap();
            CreateMap<Repo.Models.Order, CheckOrderDTO>().ReverseMap();
            CreateMap<OrderDetail, GetDetailDTO>().ReverseMap();
>>>>>>> Stashed changes
        }
    }
}
