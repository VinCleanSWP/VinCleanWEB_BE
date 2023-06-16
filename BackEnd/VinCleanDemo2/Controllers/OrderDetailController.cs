using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Order;
using VinClean.Service.DTO;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _service;
        public OrderDetailController(IOrderDetailService service)
        {
            _service = service;
        }
        // GET: api/<OrderDetailController>
        [HttpGet]
        public async Task<ActionResult<List<OrderDetailDTO>>> GetAllOrderDetail()
        {
            return Ok(await _service.GetOrderDetailList());
        }

        // GET api/<OrderDetailController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetailById(int id)
        {
            return Ok(await _service.GetOrderDetailById(id));
        }

        // POST api/<OrderDetailController>
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> AddOrderDetail(OrderDetailDTO request)
        {
            var newOrderDetail = await _service.AddOrderDetail(request);
            if (newOrderDetail.Success == false && newOrderDetail.Message == "Exist")
            {
                return Ok(newOrderDetail);
            }

            if (newOrderDetail.Success == false && newOrderDetail.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding OrderDetail {request}");
                return StatusCode(500, ModelState);
            }

            if (newOrderDetail.Success == false && newOrderDetail.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding OrderDetail {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newOrderDetail.Data);
        }

        // PUT api/<OrderDetailController>/5
        [HttpPut]
        public async Task<ActionResult> UpdateOrderDetail(OrderDetailDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateOrderDetail = await _service.UpdateOrderDetail(request);

            if (updateOrderDetail.Success == false && updateOrderDetail.Message == "NotFound")
            {
                return Ok(updateOrderDetail);
            }

            if (updateOrderDetail.Success == false && updateOrderDetail.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating OrderDetail {request}");
                return StatusCode(500, ModelState);
            }

            if (updateOrderDetail.Success == false && updateOrderDetail.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating OrderDetail {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateOrderDetail);

        }
        // DELETE api/<OrderDetailController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderDetail(int id)
        {
            var deleteOrderDetail = await _service.DeleteOrderDetail(id);


            if (deleteOrderDetail.Success == false && deleteOrderDetail.Message == "NotFound")
            {
                ModelState.AddModelError("", "OrderDetail Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteOrderDetail.Success == false && deleteOrderDetail.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting OrderDetail");
                return StatusCode(500, ModelState);
            }

            if (deleteOrderDetail.Success == false && deleteOrderDetail.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting OrderDetail");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
