using Observer.Domain.Entities;

namespace Observer.Domain.Abstractions
{
    public interface IStockObserver
    {
        public void UpdatePrice(Stock stock, decimal percentChange);
    }
}