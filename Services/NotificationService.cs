using CreativeColab.Data;
using CreativeColab.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativeColab.Services
{
    public class NotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        // Create price drop notification
        public async Task<Reminder> CreatePriceDropNotification(int userId, int gameId, decimal currentPrice, decimal targetPrice, string gameTitle)
        {
            var reminder = new Reminder
            {
                UserId = userId,
                GameId = gameId,
                Title = $"🎮 Price Drop Alert: {gameTitle}",
                Note = $"Great news! The price has dropped to ${currentPrice:F2} (Your target: ${targetPrice:F2}). Time to buy!",
                DueDate = DateTime.Now,
                User = await _context.Users.FindAsync(userId),
                Game = await _context.Games.FindAsync(gameId)
            };

            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();

            return reminder;
        }

        // Get user's active notifications
        public async Task<List<Reminder>> GetUserNotifications(int userId)
        {
            return await _context.Reminders
                .Include(r => r.Game)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.DueDate)
                .ToListAsync();
        }

        // Mark notification as completed
        public async Task MarkNotificationAsCompleted(int reminderId)
        {
            var reminder = await _context.Reminders.FindAsync(reminderId);
            if (reminder != null)
            {
                // You could add an IsCompleted field to the Reminder model
                // For now, we'll just remove it
                _context.Reminders.Remove(reminder);
                await _context.SaveChangesAsync();
            }
        }

        // Get price monitoring summary for user
        public async Task<PriceMonitoringSummary> GetPriceMonitoringSummary(int userId)
        {
            var userBookmarks = await _context.Bookmarks
                .Include(b => b.Game)
                .ThenInclude(g => g.GamePrices.OrderByDescending(gp => gp.DateTracked).Take(1))
                .ThenInclude(gp => gp.Store)
                .Where(b => b.UserId == userId && b.NotifyBelowPrice.HasValue)
                .ToListAsync();

            var summary = new PriceMonitoringSummary
            {
                TotalWatchedGames = userBookmarks.Count,
                ActiveAlerts = 0,
                PriceDrops = new List<PriceDropInfo>()
            };

            foreach (var bookmark in userBookmarks)
            {
                var latestPrice = bookmark.Game.GamePrices.FirstOrDefault();
                if (latestPrice != null)
                {
                    if (latestPrice.Price <= bookmark.NotifyBelowPrice.Value)
                    {
                        summary.ActiveAlerts++;
                    }

                    // Check if there's been a price drop
                    var previousPrice = bookmark.Game.GamePrices
                        .OrderByDescending(gp => gp.DateTracked)
                        .Skip(1)
                        .FirstOrDefault();

                    if (previousPrice != null && latestPrice.Price < previousPrice.Price)
                    {
                        summary.PriceDrops.Add(new PriceDropInfo
                        {
                            GameTitle = bookmark.Game.Title,
                            CurrentPrice = latestPrice.Price,
                            PreviousPrice = previousPrice.Price,
                            PriceDrop = previousPrice.Price - latestPrice.Price,
                            StoreName = latestPrice.Store.Name,
                            DateTracked = latestPrice.DateTracked
                        });
                    }
                }
            }

            return summary;
        }

        // Send bulk notifications for price drops
        public async Task SendBulkPriceNotifications()
        {
            var priceTrackerService = new PriceTrackerService(_context);
            var priceAlerts = await priceTrackerService.GetPriceDropAlerts();

            foreach (var alert in priceAlerts)
            {
                var bookmarks = await _context.Bookmarks
                    .Where(b => b.GameId == alert.Game.GameId && b.NotifyBelowPrice.HasValue)
                    .ToListAsync();

                foreach (var bookmark in bookmarks)
                {
                    if (alert.CurrentPrice <= bookmark.NotifyBelowPrice.Value)
                    {
                        await CreatePriceDropNotification(
                            bookmark.UserId,
                            alert.Game.GameId,
                            alert.CurrentPrice,
                            bookmark.NotifyBelowPrice.Value,
                            alert.Game.Title
                        );
                    }
                }
            }
        }
    }

    // Data transfer objects for notifications
    public class PriceMonitoringSummary
    {
        public int TotalWatchedGames { get; set; }
        public int ActiveAlerts { get; set; }
        public List<PriceDropInfo> PriceDrops { get; set; } = new List<PriceDropInfo>();
    }

    public class PriceDropInfo
    {
        public string GameTitle { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal PreviousPrice { get; set; }
        public decimal PriceDrop { get; set; }
        public string StoreName { get; set; }
        public DateTime DateTracked { get; set; }
    }
}