using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Category;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategoryList()
        {
            return Ok(await _service.GetCategoryList());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountdDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var categoryFound = await _service.GetCategoryById(id);
            if (categoryFound == null)
            {
                return NotFound();
            }
            return Ok(categoryFound);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CategoryDTO request)
        {
            /*            if(request == null)
                        {
                            return BadRequest(ModelState);
                        }
                        if(ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }*/

            var newCategory = await _service.CreateCategory(request);
            if (newCategory.Success == false && newCategory.Message == "Exist")
            {
                return Ok(newCategory);
            }

            if (newCategory.Success == false && newCategory.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Category {request}");
                return StatusCode(500, ModelState);
            }

            if (newCategory.Success == false && newCategory.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Category {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newCategory.Data);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAccount(CategoryDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateCategory = await _service.UpdateCategory(request);

            if (updateCategory.Success == false && updateCategory.Message == "NotFound")
            {
                return Ok(updateCategory);
            }

            if (updateCategory.Success == false && updateCategory.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating category {request}");
                return StatusCode(500, ModelState);
            }

            if (updateCategory.Success == false && updateCategory.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating category {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateCategory);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var deleteAccount = await _service.DeleteCategory(id);


            if (deleteAccount.Success == false && deleteAccount.Message == "NotFound")
            {
                ModelState.AddModelError("", "Category Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteAccount.Success == false && deleteAccount.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting category");
                return StatusCode(500, ModelState);
            }

            if (deleteAccount.Success == false && deleteAccount.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting category");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }
    }
}

