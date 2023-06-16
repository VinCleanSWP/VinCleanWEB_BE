using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VinClean.Repo.Models;
using VinClean.Service.DTO;
using VinClean.Service.Service;

namespace VinCleanDemo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountdDTO>>> Get()
        {
            return Ok(await _service.GetAccountList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountdDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Account>> GetById(int id)
        {
            if(id <= 0)
            {
                return BadRequest(id);
            }
            var accountFound = await _service.GetAccountById(id);
            if(accountFound == null)
            {
                return NotFound();
            }
            return Ok(accountFound);
        }

        [HttpPost]
        public async Task<ActionResult<Account>> AddAccount(AccountdDTO request)
        {
/*            if(request == null)
            {
                return BadRequest(ModelState);
            }
            if(ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var newAccount = await _service.AddAccount(request);
            if(newAccount.Success == false && newAccount.Message == "Exist")
            {
                return Ok(newAccount);
            }

            if (newAccount.Success == false && newAccount.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }

            if (newAccount.Success == false && newAccount.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Account {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newAccount.Data);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAccount(AccountdDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateAccount = await _service.UpdateAccount(request);

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
            var deleteAccount = await _service.DeleteAccount(id);


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

            return NoContent();

        }

    }
}
