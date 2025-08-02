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
    // Handles game-related actions for the MVP
    public class GamesController : Controller
    {
        private readonly AppDbContext _context;
        public GamesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Games
        public async Task<IActionResult> Index()
        {
            var games = await _context.Games
                .Include(g => g.Bookmarks)
                .Include(g => g.GamePrices)
                .ToListAsync();
            
            var viewModel = new GameViewModel
            {
                Games = games
            };
            
            return View(viewModel);
        }

        // GET: /Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            
            var game = await _context.Games
                .Include(g => g.Bookmarks)
                .ThenInclude(b => b.User)
                .Include(g => g.GamePrices)
                .ThenInclude(gp => gp.Store)
                .Include(g => g.Reminders)
                .FirstOrDefaultAsync(g => g.GameId == id);
                
            if (game == null) return NotFound();
            
            var availableUsers = await _context.Users.ToListAsync();
            var viewModel = new GameViewModel
            {
                Game = game,
                GameBookmarks = game.Bookmarks.ToList(),
                GamePrices = game.GamePrices.ToList(),
                GameReminders = game.Reminders.ToList(),
                AvailableUsers = availableUsers
            };
            
            return View(viewModel);
        }

        // GET: /Games/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Games/Create
        [HttpPost]
        // Temporarily disabled anti-forgery token for debugging
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {
            // Debug: Log that the action was called
            System.Diagnostics.Debug.WriteLine("Create POST action called");
            System.Diagnostics.Debug.WriteLine($"Game Title: {game?.Title}");
            System.Diagnostics.Debug.WriteLine($"Game Genre: {game?.Genre}");
            System.Diagnostics.Debug.WriteLine($"Game Platform: {game?.Platform}");
            
            // Debug: Log the model state errors
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    System.Diagnostics.Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return View(game);
            }

            try
            {
                System.Diagnostics.Debug.WriteLine("Adding game to context");
                if (game != null)
                {
                    _context.Add(game);
                    System.Diagnostics.Debug.WriteLine("Saving changes to database");
                    await _context.SaveChangesAsync();
                    System.Diagnostics.Debug.WriteLine("Game saved successfully");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Game is null");
                    ModelState.AddModelError("", "Game data is null.");
                    return View(game);
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                System.Diagnostics.Debug.WriteLine($"Database Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Exception Stack Trace: {ex.StackTrace}");
                ModelState.AddModelError("", "An error occurred while saving the game.");
                return View(game);
            }
        }

        // GET: /Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();
            return View(game);
        }

        // POST: /Games/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Game game)
        {
            if (id != game.GameId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: /Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var game = await _context.Games.FindAsync(id);
            if (game == null) return NotFound();
            return View(game);
        }

        // POST: /Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}