using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _service;
        public SlotController(ISlotService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Slot>>> Get()
        {
            return Ok(await _service.GetSlotList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Slot))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Slot>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var accountFound = await _service.GetSlotById(id);
            if (accountFound == null)
            {
                return NotFound();
            }
            return Ok(accountFound);
        }

        [HttpPost]
        public async Task<ActionResult<Slot>> AddAccount(Slot request)
        {
            /*            if(request == null)
                        {
                            return BadRequest(ModelState);
                        }
                        if(ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }*/

            var newSlot = await _service.AddSlot(request);
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
        public async Task<ActionResult> UpdateSlot(Slot request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateAccount = await _service.UpdateSlot(request);

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
        public async Task<ActionResult> DeleteAccount(int id)
        {
            var deleteAccount = await _service.DeleteSlot(id);


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
