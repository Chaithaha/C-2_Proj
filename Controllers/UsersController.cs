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
    // Handles user-related actions for the MVP
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Users
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Include(u => u.OwnedProjects)
                .Include(u => u.Bookmarks)
                .Include(u => u.Payments)
                .ToListAsync();
            
            var viewModel = new UserViewModel
            {
                Users = users
            };
            
            return View(viewModel);
        }

        // GET: /Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            
            var user = await _context.Users
                .Include(u => u.OwnedProjects)
                .Include(u => u.Bookmarks)
                .ThenInclude(b => b.Game)
                .Include(u => u.Payments)
                .Include(u => u.Reminders)
                .Include(u => u.DesignerStatus)
                .FirstOrDefaultAsync(u => u.UserId == id);
                
            if (user == null) return NotFound();
            
            var viewModel = new UserViewModel
            {
                User = user,
                OwnedProjects = user.OwnedProjects.ToList(),
                UserBookmarks = user.Bookmarks.ToList(),
                UserPayments = user.Payments.ToList(),
                UserReminders = user.Reminders.ToList(),
                DesignerStatus = user.DesignerStatus
            };
            
            return View(viewModel);
        }

        // GET: /Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Users/Create
        [HttpPost]
        // Temporarily disabled anti-forgery token for debugging
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            // Debug: Log that the action was called
            System.Diagnostics.Debug.WriteLine("Create POST action called for User");
            System.Diagnostics.Debug.WriteLine($"User Username: {user?.Username}");
            System.Diagnostics.Debug.WriteLine($"User Email: {user?.Email}");
            
            // Set default values for required fields if not provided
            if (user != null)
            {
                if (user.CreatedAt == default(DateTime))
                {
                    user.CreatedAt = DateTime.Now;
                    System.Diagnostics.Debug.WriteLine($"Set default CreatedAt: {user.CreatedAt}");
                }
            }
            
            // Debug: Log the model state errors
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is invalid for User");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    System.Diagnostics.Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return View(user);
            }

            try
            {
                System.Diagnostics.Debug.WriteLine("Adding user to context");
                if (user != null)
                {
                    _context.Add(user);
                    System.Diagnostics.Debug.WriteLine("Saving changes to database");
                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine("User saved successfully");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("User is null");
                    ModelState.AddModelError("", "User data is null.");
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                System.Diagnostics.Debug.WriteLine($"Database Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Exception Stack Trace: {ex.StackTrace}");
                ModelState.AddModelError("", "An error occurred while saving the user.");
                return View(user);
            }
        }

        // GET: /Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.UserId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: /Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}