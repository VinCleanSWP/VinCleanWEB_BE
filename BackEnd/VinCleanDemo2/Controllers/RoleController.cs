using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Service.DTO.Role;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RoleDTO>>> GetRoles()
        {
            var roles = await _roleService.GetRoleList();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoleDTO>> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleById(id);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult<RoleDTO>> AddRole(RoleDTO roleDTO)
        {
            var addRole = await _roleService.AddRole(roleDTO);

            if (!addRole.Success)
            {
                ModelState.AddModelError("", addRole.Message);
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction(nameof(GetRoleById), new { id = addRole.Data.RoleId }, addRole.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRole(int id, RoleDTO roleDTO)
        {
            if (id != roleDTO.RoleId)
            {
                return BadRequest();
            }

            var updatedRole = await _roleService.UpdateRole(roleDTO);

            if (!updatedRole.Success)
            {
                ModelState.AddModelError("", updatedRole.Message);
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            var deletedRole = await _roleService.DeleteRole(id);

            if (!deletedRole.Success)
            {
                ModelState.AddModelError("", deletedRole.Message);
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
