using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Employee;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinishedByController : ControllerBase
    {
        private readonly IFinishedByService _service;
        public FinishedByController(IFinishedByService service)
        {
            _service = service;
        }
        // GET: api/<FinishedByController>
        [HttpGet]
        public async Task<ActionResult<List<FinishedByDTO>>> GetAllFinishedBy()
        {
            return Ok(await _service.GetFinishedByList());
        }

        // GET api/<FinishedByController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FinshedBy>> GetFinishedByById(int id)
        {
            return Ok(await _service.GetFinishedById(id));
        }

        // POST api/<FinishedByController>
        [HttpPost]
        public async Task<ActionResult<FinshedBy>> AddFinishedBy(FinishedByDTO request)
        {
            var newFinishedBy = await _service.AddFinishedBy(request);
            if (newFinishedBy.Success == false && newFinishedBy.Message == "Exist")
            {
                Console.WriteLine(newFinishedBy);
                return Ok(newFinishedBy);
            }

            if (newFinishedBy.Success == false && newFinishedBy.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding FinishedBy {request}");
                return StatusCode(500, ModelState);
            }

            if (newFinishedBy.Success == false && newFinishedBy.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding FinishedBy {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newFinishedBy.Data);
        }

        // PUT api/<FinishedByController>/5
        [HttpPut]
        public async Task<ActionResult> UpdateFinishedBy(FinishedByDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateFinishedBy = await _service.UpdateFinishedBy(request);

            if (updateFinishedBy.Success == false && updateFinishedBy.Message == "NotFound")
            {
                return Ok(updateFinishedBy);
            }

            if (updateFinishedBy.Success == false && updateFinishedBy.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating FinishedBy {request}");
                return StatusCode(500, ModelState);
            }

            if (updateFinishedBy.Success == false && updateFinishedBy.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating FinishedBy {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateFinishedBy);

        }
        // DELETE api/<FinishedByController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFinishedBy(int id)
        {
            var deleteFinishedBy = await _service.DeleteFinishedBy(id);


            if (deleteFinishedBy.Success == false && deleteFinishedBy.Message == "NotFound")
            {
                ModelState.AddModelError("", "FinishedBy Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteFinishedBy.Success == false && deleteFinishedBy.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting FinishedBy");
                return StatusCode(500, ModelState);
            }

            if (deleteFinishedBy.Success == false && deleteFinishedBy.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting FinishedBy");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
