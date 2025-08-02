using Microsoft.AspNetCore.Mvc;
using CreativeColab.Data;
using CreativeColab.Models;
using CreativeColab.Services;
using CreativeColab.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace CreativeColab.Controllers
{
    public class PriceMonitoringController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PriceTrackerService _priceTrackerService;
        private readonly NotificationService _notificationService;

        public PriceMonitoringController(AppDbContext context)
        {
            _context = context;
            _priceTrackerService = new PriceTrackerService(context);
            _notificationService = new NotificationService(context);
        }

        // GET: /PriceMonitoring
        public async Task<IActionResult> Index()
        {
            var priceAlerts = await _priceTrackerService.GetPriceDropAlerts();
            var stores = await _context.Stores.ToListAsync();
            
            var viewModel = new PriceMonitoringViewModel
            {
                PriceAlerts = priceAlerts,
                AvailableStores = stores
            };

            return View(viewModel);
        }

        // GET: /PriceMonitoring/GamePrices/5
        public async Task<IActionResult> GamePrices(int id)
        {
            var game = await _context.Games
                .Include(g => g.GamePrices)
                .ThenInclude(gp => gp.Store)
                .FirstOrDefaultAsync(g => g.GameId == id);

            if (game == null)
                return NotFound();

            var priceHistory = await _priceTrackerService.GetGamePriceHistory(id);
            var priceStats = await _priceTrackerService.GetPriceStatistics(id);
            var currentPrices = await _priceTrackerService.GetCurrentGamePrices(id);

            var viewModel = new GamePriceViewModel
            {
                Game = game,
                PriceHistory = priceHistory,
                PriceStatistics = priceStats,
                CurrentPrices = currentPrices
            };

            return View(viewModel);
        }

        // GET: /PriceMonitoring/UserAlerts
        public async Task<IActionResult> UserAlerts()
        {
            // For demo purposes, using user ID 1
            // In a real app, this would come from authentication
            int userId = 1;

            var userAlerts = await _priceTrackerService.GetUserPriceAlerts(userId);
            var monitoringSummary = await _notificationService.GetPriceMonitoringSummary(userId);
            var userNotifications = await _notificationService.GetUserNotifications(userId);

            var viewModel = new UserPriceAlertsViewModel
            {
                PriceAlerts = userAlerts,
                MonitoringSummary = monitoringSummary,
                Notifications = userNotifications
            };

            return View(viewModel);
        }

        // POST: /PriceMonitoring/AddPrice
        [HttpPost]
        public async Task<IActionResult> AddPrice(int gameId, int storeId, decimal price)
        {
            if (price <= 0)
            {
                ModelState.AddModelError("", "Price must be greater than 0.");
                return RedirectToAction("GamePrices", new { id = gameId });
            }

            await _priceTrackerService.AddPriceEntry(gameId, storeId, price);
            
            TempData["SuccessMessage"] = "Price entry added successfully!";
            return RedirectToAction("GamePrices", new { id = gameId });
        }

        // POST: /PriceMonitoring/MarkNotificationComplete
        [HttpPost]
        public async Task<IActionResult> MarkNotificationComplete(int reminderId)
        {
            await _notificationService.MarkNotificationAsCompleted(reminderId);
            return RedirectToAction("UserAlerts");
        }

        // GET: /PriceMonitoring/PriceHistory/5
        public async Task<IActionResult> PriceHistory(int id)
        {
            var game = await _context.Games
                .Include(g => g.GamePrices)
                .ThenInclude(gp => gp.Store)
                .FirstOrDefaultAsync(g => g.GameId == id);

            if (game == null)
                return NotFound();

            var priceHistory = await _priceTrackerService.GetGamePriceHistory(id);
            
            return View(new GamePriceHistoryViewModel
            {
                Game = game,
                PriceHistory = priceHistory
            });
        }

        // GET: /PriceMonitoring/Stores
        public async Task<IActionResult> Stores()
        {
            var stores = await _context.Stores
                .Include(s => s.GamePrices)
                .ThenInclude(gp => gp.Game)
                .ToListAsync();

            return View(stores);
        }
    }
} 