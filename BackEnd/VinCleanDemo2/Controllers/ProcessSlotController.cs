using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Process;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessSlotController : ControllerBase
    {
        private readonly IProcessSlotService _service;
        public ProcessSlotController(IProcessSlotService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProcessRequestModel>>> ProcessSlot()
        {
            return Ok(await _service.GetPS());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProcessRequestModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProcessRequestModel>> GetPSById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var processFound = await _service.GetPSById(id);
            if (processFound == null)
            {
                return NotFound();
            }
            return Ok(processFound);
        }
        [HttpPost]
        public async Task<ActionResult<ProcessSlotDTO>> CreatePS(AddProcessSlot request)
        {


            var newProcess = await _service.CreatePS(request);
            if (newProcess.Success == false && newProcess.Message == "Exist")
            {
                return Ok(newProcess);
            }

            if (newProcess.Success == false && newProcess.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Process {request}");
                return StatusCode(500, ModelState);
            }

            if (newProcess.Success == false && newProcess.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Process {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newProcess.Data);
        }
        [HttpPut]
        public async Task<ActionResult> UpdatePS(ProcessSlotDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateProcess = await _service.UpdatePS(request);

            if (updateProcess.Success == false && updateProcess.Message == "NotFound")
            {
                return Ok(updateProcess);
            }

            if (updateProcess.Success == false && updateProcess.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }

            if (updateProcess.Success == false && updateProcess.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateProcess);

        }
        [HttpPut("Denied")]
        public async Task<ActionResult> Denied(ProcessSlotDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateProcess = await _service.CancelRequest(request);

            if (updateProcess.Success == false && updateProcess.Message == "NotFound")
            {
                return Ok(updateProcess);
            }

            if (updateProcess.Success == false && updateProcess.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }

            if (updateProcess.Success == false && updateProcess.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Process {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateProcess);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePS(int id)
        {
            var deleteProcess = await _service.DeletePS(id);


            if (deleteProcess.Success == false && deleteProcess.Message == "NotFound")
            {
                ModelState.AddModelError("", "Process Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteProcess.Success == false && deleteProcess.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting Process");
                return StatusCode(500, ModelState);
            }

            if (deleteProcess.Success == false && deleteProcess.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting Process");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
