using CreativeColab.Services;

namespace CreativeColab.Models.ViewModels
{
    public class PriceMonitoringViewModel
    {
        public List<GamePriceAlert> PriceAlerts { get; set; } = new List<GamePriceAlert>();
        public List<Store> AvailableStores { get; set; } = new List<Store>();
    }

    public class GamePriceViewModel
    {
        public Game Game { get; set; }
        public List<GamePrice> PriceHistory { get; set; } = new List<GamePrice>();
        public PriceStatistics PriceStatistics { get; set; }
        public List<GamePrice> CurrentPrices { get; set; } = new List<GamePrice>();
    }

    public class UserPriceAlertsViewModel
    {
        public List<UserPriceAlert> PriceAlerts { get; set; } = new List<UserPriceAlert>();
        public PriceMonitoringSummary MonitoringSummary { get; set; }
        public List<Reminder> Notifications { get; set; } = new List<Reminder>();
    }

    public class GamePriceHistoryViewModel
    {
        public Game Game { get; set; }
        public List<GamePrice> PriceHistory { get; set; } = new List<GamePrice>();
    }
} 