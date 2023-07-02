using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Process;
using VinClean.Service.Service;

namespace VinCleanDemo2.Controllers
{
    // API Deploy

    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly IProcessService _service;
        public ProcessController(IProcessService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProcessModeDTO>>> Process()
        {
            return Ok(await _service.GetProcessList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProcessDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Process>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var processFound = await _service.GetProcessById(id);
            if (processFound == null)
            {
                return NotFound();
            }
            return Ok(processFound);
        }

        [HttpGet("GetALL/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProcessModeDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProcessModeDTO>> GetAllInfo(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var processFound = await _service.GetAllInfoById(id);
            if (processFound == null)
            {
                return NotFound();
            }
            return Ok(processFound);
        }
        [HttpPost]
        public async Task<ActionResult<Process>> CreateProcess(NewBooking request)
        {


            var newProcess = await _service.AddProcess(request);
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
        public async Task<ActionResult> UpdateProcess(ProcessDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateProcess = await _service.UpdateProcess(request);

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

        [HttpPut("StartWorking")]
        public async Task<ActionResult> UpdateStartWorking(ProcessDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateProcess = await _service.UpdateStartWorking(request);

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

        [HttpPut("EndWorking")]
        public async Task<ActionResult> UpdateEndWorking(ProcessDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateProcess = await _service.UpdateEndWorking(request);

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

        [HttpPut("Status")]
        public async Task<ActionResult> UpdateStatus(ProcessDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateProcess = await _service.UpdateStatus(request);

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
        public async Task<ActionResult> DeleteProcess(int id)
        {
            var deleteProcess = await _service.DeleteProcess(id);


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
