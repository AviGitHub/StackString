using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStack.Data.Services;

namespace WebStack.Controllers
{
    /// <summary>
    /// Expose rest api for handling string stack
    ///
    /// NOTE:   all of the exceptions received from the service are being reflected to the
    ///         user as server internal error.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StackStringController : ControllerBase
    {
        private readonly ILogger<StackStringController> _logger;
        private readonly IStackStringService _service;

        public StackStringController(ILogger<StackStringController> logger, IStackStringService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Push new string to the stack
        /// </summary>
        /// <param name="stackString">content</param>
        /// <returns>200 for success, 
        /// 400 for invalid input from user (null or empty string), 
        /// 500 for internal error
        /// </returns>
        [HttpPost("[action]")]
        public IActionResult Push([FromBody] string stackString)
        {
            try
            {
                if (stackString == null)
                {
                    return BadRequest(new {message = "Input string must be non-empty"});
                }

                _service.Push(stackString);
                return Ok(new {message = "The string was pushed successfully to the stack"});
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Return the top element of the stack
        /// </summary>
        /// <returns>200 for success with the top string, 
        /// 404 if the stack is empty 
        /// 500 for internal error
        /// </returns>
        [HttpGet("[action]")]
        public IActionResult Pick()
        {
            try
            {
                var pick = _service.Pick();

                if (string.IsNullOrEmpty(pick))
                {
                    return NotFound(new {message = "Stack is empty!"});
                }

                return Ok(new {topElement = pick});
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Return the top element of the stack and remove it
        /// </summary>
        /// <returns>200 for success with the top string, 
        /// 404 if the stack is empty 
        /// 500 for internal error
        /// </returns>
        [HttpDelete("[action]")]
        public IActionResult Pop()
        {
            try
            {
                var pop = _service.Pop();
                if (string.IsNullOrEmpty(pop))
                {
                    return NotFound(new {message = "Stack is empty!"});
                }

                return Ok(new {topElement = pop});
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Revert the order of the stack (bottom element becomes top element)
        /// </summary>
        /// <returns>200 for success, 
        /// 404 if the stack is empty, 
        /// 500 for internal error
        /// </returns>
        [HttpPost("[action]")]
        public IActionResult Revert()
        {
            try
            {
                var revert = _service.Revert();

                if (revert)
                {
                    return Ok(new { message = "Stack reverted successfully" });
                }

                return BadRequest(new {message = "Stack is empty!"});
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}