using Observer.Domain.Abstractions;
using Observer.Domain.Entities;

namespace Observer.Application.Services
{
    public class SmsService : IStockObserver
    {
        public void UpdatePrice(Stock stock, decimal percentChange)
        {
            Console.WriteLine($"[Push: {stock.Symbol} agora em R$ {stock.Price:N2} ({percentChange:+0.00;-0.00}%)");
        }
    }
}
