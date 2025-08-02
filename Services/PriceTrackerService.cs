using CreativeColab.Data;
using CreativeColab.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativeColab.Services
{
    public class PriceTrackerService
    {
        private readonly AppDbContext _context;

        public PriceTrackerService(AppDbContext context)
        {
            _context = context;
        }

        // Get price history for a game
        public async Task<List<GamePrice>> GetGamePriceHistory(int gameId)
        {
            return await _context.GamePrices
                .Include(gp => gp.Store)
                .Where(gp => gp.GameId == gameId)
                .OrderByDescending(gp => gp.DateTracked)
                .ToListAsync();
        }

        // Get current prices for a game across all stores
        public async Task<List<GamePrice>> GetCurrentGamePrices(int gameId)
        {
            var latestPrices = await _context.GamePrices
                .Include(gp => gp.Store)
                .Where(gp => gp.GameId == gameId)
                .GroupBy(gp => gp.StoreId)
                .Select(g => g.OrderByDescending(gp => gp.DateTracked).First())
                .ToListAsync();

            return latestPrices;
        }

        // Add new price entry
        public async Task<GamePrice> AddPriceEntry(int gameId, int storeId, decimal price)
        {
            var priceEntry = new GamePrice
            {
                GameId = gameId,
                StoreId = storeId,
                Price = price,
                DateTracked = DateTime.Now
            };

            _context.GamePrices.Add(priceEntry);
            await _context.SaveChangesAsync();

            // Check for price alerts
            await CheckPriceAlerts(gameId, price);

            return priceEntry;
        }

        // Check if price drop triggers any bookmarks
        private async Task CheckPriceAlerts(int gameId, decimal currentPrice)
        {
            var bookmarks = await _context.Bookmarks
                .Include(b => b.User)
                .Include(b => b.Game)
                .Where(b => b.GameId == gameId && b.NotifyBelowPrice.HasValue)
                .ToListAsync();

            foreach (var bookmark in bookmarks)
            {
                if (currentPrice <= bookmark.NotifyBelowPrice.Value)
                {
                    // Create notification/reminder for price drop
                    var reminder = new Reminder
                    {
                        UserId = bookmark.UserId,
                        GameId = gameId,
                        Title = $"Price Drop Alert: {bookmark.Game.Title}",
                        Note = $"Price dropped to ${currentPrice:F2} (Target: ${bookmark.NotifyBelowPrice:F2})",
                        DueDate = DateTime.Now,
                        User = bookmark.User,
                        Game = bookmark.Game
                    };

                    _context.Reminders.Add(reminder);
                }
            }

            await _context.SaveChangesAsync();
        }

        // Get price statistics for a game
        public async Task<PriceStatistics> GetPriceStatistics(int gameId)
        {
            var prices = await _context.GamePrices
                .Where(gp => gp.GameId == gameId)
                .ToListAsync();

            if (!prices.Any())
                return new PriceStatistics();

            var currentPrice = prices.OrderByDescending(p => p.DateTracked).First().Price;
            var minPrice = prices.Min(p => p.Price);
            var maxPrice = prices.Max(p => p.Price);
            var avgPrice = prices.Average(p => p.Price);

            var previousPrice = prices
                .OrderByDescending(p => p.DateTracked)
                .Skip(1)
                .FirstOrDefault()?.Price;

            decimal? priceChange = null;
            double? priceChangePercentage = null;

            if (previousPrice.HasValue)
            {
                priceChange = currentPrice - previousPrice.Value;
                priceChangePercentage = (double)((priceChange.Value / previousPrice.Value) * 100);
            }

            return new PriceStatistics
            {
                CurrentPrice = currentPrice,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                AveragePrice = avgPrice,
                PriceChange = priceChange,
                PriceChangePercentage = priceChangePercentage,
                TotalPriceEntries = prices.Count
            };
        }

        // Get games with price drops
        public async Task<List<GamePriceAlert>> GetPriceDropAlerts()
        {
            var alerts = new List<GamePriceAlert>();

            var games = await _context.Games
                .Include(g => g.GamePrices)
                .ThenInclude(gp => gp.Store)
                .Include(g => g.Bookmarks)
                .ToListAsync();

            foreach (var game in games)
            {
                var priceHistory = game.GamePrices.OrderByDescending(p => p.DateTracked).Take(2).ToList();
                if (priceHistory.Count >= 2)
                {
                    var currentPrice = priceHistory[0].Price;
                    var previousPrice = priceHistory[1].Price;

                    if (currentPrice < previousPrice)
                    {
                        var priceDrop = previousPrice - currentPrice;
                        var dropPercentage = (priceDrop / previousPrice) * 100;

                        alerts.Add(new GamePriceAlert
                        {
                            Game = game,
                            CurrentPrice = currentPrice,
                            PreviousPrice = previousPrice,
                            PriceDrop = priceDrop,
                            DropPercentage = dropPercentage,
                            Store = priceHistory[0].Store,
                            DateTracked = priceHistory[0].DateTracked
                        });
                    }
                }
            }

            return alerts.OrderByDescending(a => a.DropPercentage).ToList();
        }

        // Get user's price alerts
        public async Task<List<UserPriceAlert>> GetUserPriceAlerts(int userId)
        {
            var userBookmarks = await _context.Bookmarks
                .Include(b => b.Game)
                .ThenInclude(g => g.GamePrices.OrderByDescending(gp => gp.DateTracked).Take(1))
                .ThenInclude(gp => gp.Store)
                .Where(b => b.UserId == userId && b.NotifyBelowPrice.HasValue)
                .ToListAsync();

            var alerts = new List<UserPriceAlert>();

            foreach (var bookmark in userBookmarks)
            {
                var latestPrice = bookmark.Game.GamePrices.FirstOrDefault();
                if (latestPrice != null && latestPrice.Price <= bookmark.NotifyBelowPrice.Value)
                {
                    alerts.Add(new UserPriceAlert
                    {
                        Bookmark = bookmark,
                        CurrentPrice = latestPrice.Price,
                        TargetPrice = bookmark.NotifyBelowPrice.Value,
                        Store = latestPrice.Store,
                        DateTracked = latestPrice.DateTracked
                    });
                }
            }

            return alerts;
        }
    }

    // Data transfer objects for price tracking
    public class PriceStatistics
    {
        public decimal CurrentPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal? PriceChange { get; set; }
        public double? PriceChangePercentage { get; set; }
        public int TotalPriceEntries { get; set; }
    }

    public class GamePriceAlert
    {
        public Game Game { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal PreviousPrice { get; set; }
        public decimal PriceDrop { get; set; }
        public decimal DropPercentage { get; set; }
        public Store Store { get; set; }
        public DateTime DateTracked { get; set; }
    }

    public class UserPriceAlert
    {
        public Bookmark Bookmark { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal TargetPrice { get; set; }
        public Store Store { get; set; }
        public DateTime DateTracked { get; set; }
    }
}