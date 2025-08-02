using Microsoft.AspNetCore.Mvc;
using CreativeColab.Data;
using CreativeColab.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CreativeColab.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentsAPIController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentsAPIController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            var payments = await _context.Payments.ToListAsync();
            return Ok(payments);
        }

        // GET: api/payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        // POST: api/payments
        [HttpPost]
        public async Task<ActionResult<Payment>> CreatePayment([FromBody] Payment payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentId }, payment);
        }

        // PUT: api/payments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] Payment payment)
        {
            if (id != payment.PaymentId)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Update(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/payments/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetUserPayments(int userId)
        {
            var payments = await _context.Payments
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return Ok(payments);
        }

        // GET: api/payments/project/5
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetProjectPayments(int projectId)
        {
            var payments = await _context.Payments
                .Where(p => p.ProjectId == projectId)
                .ToListAsync();

            return Ok(payments);
        }
    }
} 