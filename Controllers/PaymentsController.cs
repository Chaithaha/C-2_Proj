using Microsoft.AspNetCore.Mvc;
using CreativeColab.Data;
using CreativeColab.Models;
using CreativeColab.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace CreativeColab.Controllers
{
    // Handles payment-related actions for the MVP
    public class PaymentsController : Controller
    {
        private readonly AppDbContext _context;
        public PaymentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Payments
        public async Task<IActionResult> Index()
        {
            var payments = await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Project)
                .ToListAsync();
            
            var viewModel = new PaymentViewModel
            {
                Payments = payments
            };
            
            return View(viewModel);
        }

        // GET: /Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            
            var payment = await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Project)
                .FirstOrDefaultAsync(p => p.PaymentId == id);
                
            if (payment == null) return NotFound();
            
            var viewModel = new PaymentViewModel
            {
                Payment = payment
            };
            
            return View(viewModel);
        }

        // GET: /Payments/Create
        public async Task<IActionResult> Create()
        {
            var availableUsers = await _context.Users.ToListAsync();
            var availableProjects = await _context.Projects.ToListAsync();
            var viewModel = new PaymentViewModel
            {
                AvailableUsers = availableUsers,
                AvailableProjects = availableProjects
            };
            return View(viewModel);
        }

        // POST: /Payments/Create
        [HttpPost]
        // Temporarily disabled anti-forgery token for debugging
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment payment)
        {
            // Debug: Log that the action was called
            System.Diagnostics.Debug.WriteLine("Create POST action called for Payment");
            System.Diagnostics.Debug.WriteLine($"Payment Amount: {payment?.Amount}");
            System.Diagnostics.Debug.WriteLine($"Payment Description: {payment?.Description}");
            
            // Set default values for required fields if not provided
            if (payment != null)
            {
                if (payment.UserId == 0)
                {
                    payment.UserId = 1; // Default to user ID 1
                    System.Diagnostics.Debug.WriteLine($"Set default UserId: {payment.UserId}");
                }
                
                if (payment.PaymentDate == default(DateTime))
                {
                    payment.PaymentDate = DateTime.Now;
                    System.Diagnostics.Debug.WriteLine($"Set default PaymentDate: {payment.PaymentDate}");
                }
                
                // Ensure the user exists before creating the payment
                var userExists = await _context.Users.AnyAsync(u => u.UserId == payment.UserId);
                if (!userExists)
                {
                    System.Diagnostics.Debug.WriteLine($"User {payment.UserId} does not exist, creating default user");
                    var defaultUser = new User
                    {
                        Username = "default_user",
                        Email = "default@example.com",
                        PasswordHash = "default_password_hash",
                        Role = "user",
                        CreatedAt = DateTime.Now
                    };
                    _context.Users.Add(defaultUser);
                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine($"Created default user with ID: {defaultUser.UserId}");
                }
            }
            
            // Debug: Log the model state errors
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is invalid for Payment");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    System.Diagnostics.Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return View(payment);
            }

            try
            {
                System.Diagnostics.Debug.WriteLine("Adding payment to context");
                if (payment != null)
                {
                    _context.Add(payment);
                    System.Diagnostics.Debug.WriteLine("Saving changes to database");
                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine("Payment saved successfully");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Payment is null");
                    ModelState.AddModelError("", "Payment data is null.");
                    return View(payment);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                System.Diagnostics.Debug.WriteLine($"Database Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Exception Stack Trace: {ex.StackTrace}");
                ModelState.AddModelError("", "An error occurred while saving the payment.");
                return View(payment);
            }
        }

        // GET: /Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();
            return View(payment);
        }

        // POST: /Payments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Payment payment)
        {
            if (id != payment.PaymentId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
        }

        // GET: /Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();
            return View(payment);
        }

        // POST: /Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}