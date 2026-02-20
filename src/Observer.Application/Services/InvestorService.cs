using Observer.Domain.Abstractions;
using Observer.Domain.Entities;

namespace Observer.Application.Services
{
    public class InvestorService : IStockObserver
    {
        public void UpdatePrice(Stock stock, decimal percentChange)
        {
            var investorsDatabase = new List<Investor>() { new("Investidor 1", 1), new("Investidor 2", 2) };

            foreach (var investor in investorsDatabase)
                Notify(stock, investor, percentChange);
        }

        public static void Notify(Stock stock, Investor investor, decimal percentChange)
        {
            Console.WriteLine($"[Investidor {investor.Name}] Notificado sobre {stock.Symbol}");

            if (Math.Abs(percentChange) >= investor.AlertThreshold)
                Console.WriteLine($"[Investidor {investor.Name}] ALERTA! Mudança de {percentChange:+0.00;-0.00}% excedeu limite de {investor.AlertThreshold}%");
        }
    }
}