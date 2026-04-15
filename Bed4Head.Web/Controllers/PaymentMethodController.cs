using Bed4Head.Application.DTOs;
using Bed4Head.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bed4Head.Web.Controllers
{
    [Route("api/payment-methods")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentService;

        public PaymentMethodController(IPaymentMethodService paymentService)
        {
            _paymentService = paymentService;
        }

        // --- USER METHODS ---

        // Get all payment methods for a specific user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var methods = await _paymentService.GetByUserIdAsync(userId);
            return Ok(methods);
        }

        // Get specific payment method details
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var method = await _paymentService.GetByIdAsync(id);
            if (method == null)
            {
                return NotFound(new { message = "Payment method not found" });
            }
            return Ok(method);
        }

        // Add a new payment method (e.g., link a card)
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PaymentMethodDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _paymentService.CreateAsync(dto);
            return Ok(new { message = "Payment method added successfully" });
        }

        // Update card details (like primary status or expiry date)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PaymentMethodDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            await _paymentService.UpdateAsync(dto);
            return Ok(new { message = "Payment method updated successfully" });
        }

        // Remove a card
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _paymentService.DeleteAsync(id);
            return Ok(new { message = "Payment method deleted successfully" });
        }
    }
}


