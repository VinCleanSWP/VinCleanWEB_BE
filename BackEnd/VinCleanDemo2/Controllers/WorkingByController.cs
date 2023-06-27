using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO.WorkingSlot;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingByController : ControllerBase
    {
        private readonly IWorkingByService _service;
        public WorkingByController(IWorkingByService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<WorkingByDTO>>> Get()
        {
            return Ok(await _service.GetWBList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WorkingByDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<WorkingByDTO>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var accountFound = await _service.GetWBById(id);
            if (accountFound == null)
            {
                return NotFound();
            }
            return Ok(accountFound);
        }

        [HttpPost]
        public async Task<ActionResult<WorkingByDTO>> AddWB(WorkingByDTO request)
        {
            /*            if(request == null)
                        {
                            return BadRequest(ModelState);
                        }
                        if(ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }*/

            var newSlot = await _service.AddWB(request);
            if (newSlot.Success == false && newSlot.Message == "Exist")
            {
                return Ok(newSlot);
            }

            if (newSlot.Success == false && newSlot.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }

            if (newSlot.Success == false && newSlot.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newSlot.Data);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateWB(WorkingByDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateAccount = await _service.UpdateWB(request);

            if (updateAccount.Success == false && updateAccount.Message == "NotFound")
            {
                return Ok(updateAccount);
            }

            if (updateAccount.Success == false && updateAccount.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating account {request}");
                return StatusCode(500, ModelState);
            }

            if (updateAccount.Success == false && updateAccount.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating account {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateAccount);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWB(int id)
        {
            var deleteAccount = await _service.DeleteWB(id);


            if (deleteAccount.Success == false && deleteAccount.Message == "NotFound")
            {
                ModelState.AddModelError("", "Account Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteAccount.Success == false && deleteAccount.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting account");
                return StatusCode(500, ModelState);
            }

            if (deleteAccount.Success == false && deleteAccount.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting account");
                return StatusCode(500, ModelState);
            }

            return Ok(deleteAccount);

        }
    }
}
