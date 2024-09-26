using Microsoft.AspNetCore.Mvc;
using SportShopC_.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using SportShopC_.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace SportShopC_.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(ILogger<OrderController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Order/GetAllOrders
        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _unitOfWork._orderRepository.GetAllAsync();
                _unitOfWork.Commit();
                _logger.LogInformation("Returned all orders from database.");
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Error in GetAllOrdersAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Order/GetById/{id}
        [HttpGet("GetById/{id}", Name = "GetOrderByIdAsync")]
        public async Task<ActionResult<Order>> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _unitOfWork._orderRepository.GetAsync(id);
                if (order == null)
                {
                    _logger.LogError($"Order with id: {id} not found.");
                    return NotFound();
                }
                _logger.LogInformation($"Returned order with id: {id}");
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetOrderByIdAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Order/GetByUserId/{userId}
        [HttpGet("GetByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserIdAsync(int userId)
        {
            try
            {
                var orders = await _unitOfWork._orderRepository.GetOrdersByUserIdAsync(userId);
                _unitOfWork.Commit();
                if (orders == null)
                {
                    _logger.LogError($"No orders found for user with id: {userId}");
                    return NotFound();
                }
                _logger.LogInformation($"Returned orders for user with id: {userId}");
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetOrdersByUserIdAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Order/CreateOrder
        [HttpPost("CreateOrder",Name = "CreateOrder") ]
        public async Task<ActionResult> CreateOrderAsync([FromBody] Order newOrder)
        {
            try
            {
                if (newOrder == null)
                {
                    _logger.LogError("Order object is null.");
                    return BadRequest("Order object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid order object.");
                    return BadRequest("Invalid model object");
                }
                var createdId = await _unitOfWork._orderRepository.AddAsync(newOrder);
                var createdOrder = await _unitOfWork._orderRepository.GetAsync(createdId);
                _unitOfWork.Commit();
                //return Ok(createdOrder);
                return CreatedAtAction(nameof(GetOrderByIdAsync), new { id = createdId }, createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateOrderAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/Order/UpdateOrder/{id}
        [HttpPut("UpdateOrder/{id}")]
        public async Task<ActionResult> UpdateOrderAsync(int id, [FromBody] Order updatedOrder)
        {
            try
            {
                if (updatedOrder == null)
                {
                    _logger.LogError("Order object is null.");
                    return BadRequest("Order object is null");
                }
                var existingOrder = await _unitOfWork._orderRepository.GetAsync(id);
                if (existingOrder == null)
                {
                    _logger.LogError($"Order with id: {id} not found.");
                    return NotFound();
                }
                await _unitOfWork._orderRepository.ReplaceAsync(updatedOrder);
                _unitOfWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateOrderAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/Order/DeleteOrder/{id}
        [HttpDelete("DeleteOrder/{id}")]
        public async Task<ActionResult> DeleteOrderAsync(int id)
        {
            try
            {
                var existingOrder = await _unitOfWork._orderRepository.GetAsync(id);
                if (existingOrder == null)
                {
                    _logger.LogError($"Order with id: {id} not found.");
                    return NotFound();
                }
                await _unitOfWork._orderRepository.DeleteAsync(id);
                _unitOfWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteOrderAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
