﻿using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Employee;
using VinClean.Service.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }
        // GET: api/<EmployeeController>
        [HttpGet]
        public async Task<ActionResult<List<EmployeeDTO>>> GetAllEmployee()
        {
            return Ok(await _service.GetEmployeeList());
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var EmployeeFound = await _service.GetEmployeeById(id);
            if (EmployeeFound == null)
            {
                return NotFound();
            }
            return Ok(EmployeeFound);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(EmployeeDTO request)
        {
            var newEmployee = await _service.AddEmployee(request);
            if (newEmployee.Success == false && newEmployee.Message == "Exist")
            {
                return Ok(newEmployee);
            }

            if (newEmployee.Success == false && newEmployee.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Employee {request}");
                return StatusCode(500, ModelState);
            }

            if (newEmployee.Success == false && newEmployee.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Employee {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newEmployee.Data);
        }

        // PUT api/<EmployeeController>/5
        [HttpPut]
        public async Task<ActionResult> UpdateEmployee(EmployeeDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateEmployee = await _service.UpdateEmployee(request);

            if (updateEmployee.Success == false && updateEmployee.Message == "NotFound")
            {
                return Ok(updateEmployee);
            }

            if (updateEmployee.Success == false && updateEmployee.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Employee {request}");
                return StatusCode(500, ModelState);
            }

            if (updateEmployee.Success == false && updateEmployee.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Employee {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateEmployee);

        }
        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var deleteEmployee = await _service.DeleteEmployee(id);


            if (deleteEmployee.Success == false && deleteEmployee.Message == "NotFound")
            {
                ModelState.AddModelError("", "Employee Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteEmployee.Success == false && deleteEmployee.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting Employee");
                return StatusCode(500, ModelState);
            }

            if (deleteEmployee.Success == false && deleteEmployee.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting Employee");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpGet("ListViewProfile")]
        public async Task<ActionResult<List<Employee>>> GetEProfileList()
        {
            return Ok(await _service.GetEProfileList());
        }


        [HttpGet("GetProfileBy {id}")]
        public async Task<ActionResult<Employee>> GetEProfileById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var eprofile = await _service.GetEProfileById(id);
            if (eprofile == null)
            {
                return NotFound();
            }
            return Ok(eprofile);
        }

        [HttpPut("ModifyProfile")]
        public async Task<ActionResult> ModifyEProfile(ModifyEmployeeProfileDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var modifypEmployee = await _service.ModifyEProfile(request);

            if (modifypEmployee.Success == false && modifypEmployee.Message == "NotFound")
            {
                return Ok(modifypEmployee);
            }

            if (modifypEmployee.Success == false && modifypEmployee.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Employee {request}");
                return StatusCode(500, ModelState);
            }

            if (modifypEmployee.Success == false && modifypEmployee.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Employee {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(modifypEmployee);
        }
    }
}
