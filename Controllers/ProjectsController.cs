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
    // Handles project-related actions for the MVP
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _context;
        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Projects
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Include(p => p.OwnerUser)
                .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.User)
                .ToListAsync();
            
            var viewModel = new ProjectViewModel
            {
                Projects = projects
            };
            
            return View(viewModel);
        }

        // GET: /Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            
            var project = await _context.Projects
                .Include(p => p.OwnerUser)
                .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.User)
                .Include(p => p.ProjectDeadlines)
                .Include(p => p.Payments)
                .FirstOrDefaultAsync(p => p.ProjectId == id);
                
            if (project == null) return NotFound();
            
            var viewModel = new ProjectViewModel
            {
                Project = project,
                ProjectUsers = project.ProjectUsers.ToList(),
                ProjectDeadlines = project.ProjectDeadlines.ToList(),
                ProjectPayments = project.Payments.ToList()
            };
            
            return View(viewModel);
        }

        // GET: /Projects/Create
        public async Task<IActionResult> Create()
        {
            var availableUsers = await _context.Users.ToListAsync();
            var viewModel = new ProjectViewModel
            {
                AvailableUsers = availableUsers
            };
            return View(viewModel);
        }

        // POST: /Projects/Create
        [HttpPost]
        // Temporarily disabled anti-forgery token for debugging
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            // Debug: Log that the action was called
            System.Diagnostics.Debug.WriteLine("=== PROJECT CREATE POST ACTION STARTED ===");
            System.Diagnostics.Debug.WriteLine($"Project Title: {project?.Title}");
            System.Diagnostics.Debug.WriteLine($"Project Description: {project?.Description}");
            System.Diagnostics.Debug.WriteLine($"Project Status: {project?.Status}");
            System.Diagnostics.Debug.WriteLine($"Project OwnerUserId: {project?.OwnerUserId}");
            System.Diagnostics.Debug.WriteLine($"Project CreatedAt: {project?.CreatedAt}");
            
            // Set default values for required fields if not provided
            if (project != null)
            {
                if (project.OwnerUserId == 0)
                {
                    project.OwnerUserId = 1; // Default to user ID 1
                    System.Diagnostics.Debug.WriteLine($"Set default OwnerUserId: {project.OwnerUserId}");
                }
                
                if (project.CreatedAt == default(DateTime))
                {
                    project.CreatedAt = DateTime.Now;
                    System.Diagnostics.Debug.WriteLine($"Set default CreatedAt: {project.CreatedAt}");
                }
                
                // Ensure the user exists before creating the project
                var userExists = await _context.Users.AnyAsync(u => u.UserId == project.OwnerUserId);
                if (!userExists)
                {
                    System.Diagnostics.Debug.WriteLine($"User {project.OwnerUserId} does not exist, creating default user");
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
                System.Diagnostics.Debug.WriteLine("=== MODEL STATE IS INVALID ===");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    System.Diagnostics.Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return View(project);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("=== MODEL STATE IS VALID ===");
            }

            try
            {
                System.Diagnostics.Debug.WriteLine("=== ATTEMPTING DATABASE OPERATION ===");
                System.Diagnostics.Debug.WriteLine("Adding project to context");
                if (project != null)
                {
                    _context.Add(project);
                    System.Diagnostics.Debug.WriteLine("Saving changes to database");
                    var result = await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine($"SaveChangesAsync returned: {result} rows affected");
                    System.Diagnostics.Debug.WriteLine("Project saved successfully");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Project is null");
                    ModelState.AddModelError("", "Project data is null.");
                    return View(project);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                System.Diagnostics.Debug.WriteLine("=== DATABASE ERROR OCCURRED ===");
                System.Diagnostics.Debug.WriteLine($"Database Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Exception Stack Trace: {ex.StackTrace}");
                ModelState.AddModelError("", "An error occurred while saving the project.");
                return View(project);
            }
        }

        // GET: /Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        // POST: /Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            if (id != project.ProjectId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: /Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        // POST: /Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}