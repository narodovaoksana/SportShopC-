using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportShopC_.Entities;
using SportShopC_.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SportShopC_.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(ILogger<UserController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // GET: api/User/GetAllUsers
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _unitOfWork._userRepository.GetAllAsync();
                _unitOfWork.Commit();
                _logger.LogInformation("Returned all users from database.");
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllUsersAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/User/GetById/{id}
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<User>> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _unitOfWork._userRepository.GetAsync(id);
                _unitOfWork.Commit();
                if (user == null)
                {
                    _logger.LogError($"User with id: {id} not found.");
                    return NotFound();
                }
                _logger.LogInformation($"Returned user with id: {id}");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUserByIdAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/User/GetByEmail/{email}
        [HttpGet("GetByEmail/{email}")]
        public async Task<ActionResult<User>> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _unitOfWork._userRepository.GetUserByEmailAsync(email);
                _unitOfWork.Commit();
                if (user == null)
                {
                    _logger.LogError($"User with email: {email} not found.");
                    return NotFound();
                }
                _logger.LogInformation($"Returned user with email: {email}");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUserByEmailAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/User/CreateUser
        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUserAsync([FromBody] User newUser)
        {
            try
            {
                if (newUser == null)
                {
                    _logger.LogError("User object is null.");
                    return BadRequest("User object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid user object.");
                    return BadRequest("Invalid model object");
                }
                var createdId = await _unitOfWork._userRepository.AddAsync(newUser);
                var createdUser = await _unitOfWork._userRepository.GetAsync(createdId);
                _unitOfWork.Commit();
                return CreatedAtAction(nameof(GetUserByIdAsync), new { id = createdId }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateUserAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/User/UpdateUser/{id}
        [HttpPut("UpdateUser/{id}")]
        public async Task<ActionResult> UpdateUserAsync(int id, [FromBody] User updatedUser)
        {
            try
            {
                if (updatedUser == null)
                {
                    _logger.LogError("User object is null.");
                    return BadRequest("User object is null");
                }
                var existingUser = await _unitOfWork._userRepository.GetAsync(id);
                if (existingUser == null)
                {
                    _logger.LogError($"User with id: {id} not found.");
                    return NotFound();
                }
                await _unitOfWork._userRepository.ReplaceAsync(updatedUser);
                _unitOfWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateUserAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/User/DeleteUser/{id}
        [HttpDelete("DeleteUser/{id}")]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            try
            {
                var existingUser = await _unitOfWork._userRepository.GetAsync(id);
                if (existingUser == null)
                {
                    _logger.LogError($"User with id: {id} not found.");
                    return NotFound();
                }
                await _unitOfWork._userRepository.DeleteAsync(id);
                _unitOfWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteUserAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
