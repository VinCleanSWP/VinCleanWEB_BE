using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Service.DTO.Service;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ISvcService _svcService;

        public ServiceController(ISvcService svcService)
        {
            _svcService = svcService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ServicesDTO>>> GetServices()
        {
            var services = await _svcService.GetServiceList();
            return Ok(services);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServicesDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServicesDTO>> GetServiceById(int id)
        {
            var service = await _svcService.GetServiceById(id);

            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        [HttpPost]
        public async Task<ActionResult<ServicesDTO>> AddService(ServicesDTO serviceDTO)
        {
            var addService = await _svcService.AddService(serviceDTO);

            if (!addService.Success)
            {
                ModelState.AddModelError("", addService.Message);
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction(nameof(GetServiceById), new { id = addService.Data.ServiceId }, addService.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateService(int id, ServicesDTO serviceDTO)
        {
            if (id != serviceDTO.ServiceId)
            {
                return BadRequest();
            }

            var updatedService = await _svcService.UpdateService(serviceDTO);

            if (!updatedService.Success)
            {
                ModelState.AddModelError("", updatedService.Message);
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteService(int id)
        {
            var deletedService = await _svcService.DeleteService(id);

            if (!deletedService.Success)
            {
                ModelState.AddModelError("", deletedService.Message);
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}

