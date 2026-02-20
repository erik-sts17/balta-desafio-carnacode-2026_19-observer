using Observer.Domain.Abstractions;

namespace Observer.Domain.Entities
{
    public class Stock(string symbol, decimal price, DateTime lastUpdate)
    {
        private readonly List<IStockObserver> _observers = [];

        public string Symbol { get; set; } = symbol;
        public decimal Price { get; private set; } = price;
        public DateTime LastUpdate { get; private set; } = lastUpdate;

        public void UpdatePrice(decimal newPrice)
        {
            if (Price != newPrice)
            {
                Price = newPrice;
                LastUpdate = DateTime.Now;
            }
        }
    }
}