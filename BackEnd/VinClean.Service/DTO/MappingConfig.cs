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

using VinClean.Service.DTO.Process;
using VinClean.Service.DTO.Rating;
using VinClean.Service.DTO.Roles;
using VinClean.Service.DTO.Service;

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
           
           CreateMap<VinClean.Repo.Models.Employee, EmployeeProfileDTO>().ReverseMap();
            CreateMap<VinClean.Repo.Models.Employee, ModifyEmployeeProfileDTO>().ReverseMap();
            CreateMap<Customer, CustomerProfileDTO>().ReverseMap();
            CreateMap<Customer, ModifyCustomerProfileDTO>().ReverseMap();
            
            CreateMap<Role, RolesDTO>().ReverseMap();
            CreateMap<Repo.Models.Service, ServicesDTO>().ReverseMap();
            CreateMap<Repo.Models.ServiceManage, SvcManageDTO>().ReverseMap();

            CreateMap<Repo.Models.Process, ProcessDTO>().ReverseMap();
            CreateMap<Repo.Models.Rating, RatingDTO>().ReverseMap();
            CreateMap<ProcessDetail, ProcessDetailDTO>().ReverseMap();
            CreateMap<ProcessSlot, ProcessSlotDTO>().ReverseMap();

        }
    }
}
