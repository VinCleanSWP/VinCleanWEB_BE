using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Service;
using VinClean.Service.DTO.ServiceManage;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceManageController : ControllerBase
    {
        private readonly ISvcManageService _service;

        public ServiceManageController(ISvcManageService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult<List<SvcManageDTO>>> GetServiceManages()
        {
            var serviceManages = await _service.GetServiceManages();
            return Ok(serviceManages);
        }

        [HttpGet("{employeeId}/{serviceId}")]
        public async Task<ActionResult<SvcManageDTO>> GetServiceManageById(int employeeId, int serviceId)
        {
            var serviceManage = await _service.GetServiceManageById(employeeId, serviceId);

            if (serviceManage == null)
            {
                return NotFound();
            }

            return Ok(serviceManage);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<SvcManageDTO>>> AddServiceManage(SvcManageDTO serviceManageDTO)
        {
            var response = await _service.AddServiceManage(serviceManageDTO);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<SvcManageDTO>>> UpdateServiceManage(SvcManageDTO serviceManageDTO)
        {
            var response = await _service.UpdateServiceManage(serviceManageDTO);
            return Ok(response);
        }

        [HttpDelete("{employeeId}/{serviceId}")]
        public async Task<ActionResult<ServiceResponse<SvcManageDTO>>> DeleteServiceManage(int employeeId, int serviceId)
        {
            var response = await _service.DeleteServiceManage(employeeId, serviceId);
            return Ok(response);
        }
    }
}
