using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

using VinClean.Service.DTO.Account;
using VinClean.Service.DTO.CustomerResponse;

using VinClean.Service.DTO.Employee;
using VinClean.Service.DTO.Order;


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

            CreateMap<Account, AccountdDTO>().ReverseMap();
            CreateMap<VinClean.Repo.Models.Employee, EmployeeDTO>().ReverseMap();
            CreateMap<VinClean.Repo.Models.Order, OrderDTO>().ReverseMap();
            CreateMap<OrderDetail,OrderDetailDTO>().ReverseMap();
            CreateMap<VinClean.Repo.Models.FinshedBy, FinishedByDTO>().ReverseMap();
        }
    }
}
