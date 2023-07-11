using Microsoft.AspNetCore.Mvc;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Order;
using VinClean.Service.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }
        // GET: api/<OrderController>
        [HttpGet]
        public async Task<ActionResult<List<OrderModelDTO>>> GetAllOrder()
        {
            return Ok(await _service.GetOrderList());
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModelDTO>> GetOrderById(int id)
        {
            return Ok(await _service.GetOrderById(id));
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> AddOrder(NewOderDTO request)
        {
            var newOrder = await _service.AddOrder(request);
            if (newOrder.Success == false && newOrder.Message == "Exist")
            {
                return Ok(newOrder);
            }

            if (newOrder.Success == false && newOrder.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Order {request}");
                return StatusCode(500, ModelState);
            }

            if (newOrder.Success == false && newOrder.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Order {request}");
                return StatusCode(500, ModelState);
            }
            return Ok(newOrder.Data);
        }

        // PUT api/<OrderController>/5
        [HttpPut]
        public async Task<ActionResult> UpdateOrder(OrderDTO request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }


            var updateOrder = await _service.UpdateOrder(request);

            if (updateOrder.Success == false && updateOrder.Message == "NotFound")
            {
                return Ok(updateOrder);
            }

            if (updateOrder.Success == false && updateOrder.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when updating Order {request}");
                return StatusCode(500, ModelState);
            }

            if (updateOrder.Success == false && updateOrder.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when updating Order {request}");
                return StatusCode(500, ModelState);
            }


            return Ok(updateOrder);

        }
        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var deleteOrder = await _service.DeleteOrder(id);


            if (deleteOrder.Success == false && deleteOrder.Message == "NotFound")
            {
                ModelState.AddModelError("", "Order Not found");
                return StatusCode(404, ModelState);
            }

            if (deleteOrder.Success == false && deleteOrder.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in Repository when deleting Order");
                return StatusCode(500, ModelState);
            }

            if (deleteOrder.Success == false && deleteOrder.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when deleting Order");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }
    }
}
