using Observer.Domain.Abstractions;
using Observer.Domain.Entities;

namespace Observer.Application.Services
{
    public class StockService : IStockSubject
    {
        private readonly List<IStockObserver> _observers = [];

        public void Subscribe(IStockObserver observer) =>
            _observers.Add(observer);

        public void Unsubscribe(IStockObserver observer) =>
            _observers.Remove(observer);

        public void UpdatePrice(Stock stock)
        {
            var stockDatabase = new Stock("CDB", 130m, DateTime.Now.AddDays(-1));

            var changePercent = (stock.Price - stockDatabase.Price) / stockDatabase.Price * 100;

            stockDatabase.UpdatePrice(stock.Price);

            Notify(stock, changePercent);
        }

        public void Notify(Stock stock, decimal percentChange)
        {
            foreach (var observer in _observers)
                observer.UpdatePrice(stock, percentChange);
        }
    }
}