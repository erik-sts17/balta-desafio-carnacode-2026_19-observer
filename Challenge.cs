// DESAFIO: Sistema de Monitoramento de Ações na Bolsa
// PROBLEMA: Um sistema financeiro precisa notificar múltiplos investidores quando o preço
// de ações muda. O código atual faz polling constante ou tem dependências diretas entre
// as ações e os investidores, criando acoplamento forte e código difícil de manter

using System;
using System.Collections.Generic;
using System.Threading;

namespace DesignPatternChallenge
{
    // Contexto: Sistema de trading onde investidores querem ser notificados de mudanças
    // em tempo real sem ter que ficar consultando constantemente (polling)
    
    public class Stock
    {
        public string Symbol { get; set; }
        public decimal Price { get; private set; }
        public DateTime LastUpdate { get; private set; }

        // Problema: Referências diretas para investidores (acoplamento forte)
        private Investor _investor1;
        private Investor _investor2;
        private MobileApp _mobileApp;
        private TradingBot _tradingBot;

        public Stock(string symbol, decimal initialPrice)
        {
            Symbol = symbol;
            Price = initialPrice;
            LastUpdate = DateTime.Now;
        }

        // Problema: Métodos para registrar cada tipo de observador
        public void RegisterInvestor1(Investor investor)
        {
            _investor1 = investor;
        }

        public void RegisterInvestor2(Investor investor)
        {
            _investor2 = investor;
        }

        public void RegisterMobileApp(MobileApp app)
        {
            _mobileApp = app;
        }

        public void RegisterTradingBot(TradingBot bot)
        {
            _tradingBot = bot;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (Price != newPrice)
            {
                decimal oldPrice = Price;
                Price = newPrice;
                LastUpdate = DateTime.Now;
                
                decimal changePercent = ((newPrice - oldPrice) / oldPrice) * 100;
                
                Console.WriteLine($"\n[{Symbol}] Preço atualizado: R$ {oldPrice:N2} → R$ {newPrice:N2} ({changePercent:+0.00;-0.00}%)");

                // Problema: Precisa notificar cada observador manualmente
                // e conhecer o tipo específico de cada um
                if (_investor1 != null)
                {
                    _investor1.OnPriceChanged(Symbol, newPrice, changePercent);
                }

                if (_investor2 != null)
                {
                    _investor2.OnPriceChanged(Symbol, newPrice, changePercent);
                }

                if (_mobileApp != null)
                {
                    _mobileApp.SendPushNotification(Symbol, newPrice, changePercent);
                }

                if (_tradingBot != null)
                {
                    _tradingBot.AnalyzeAndTrade(Symbol, newPrice, changePercent);
                }

                // Problema: Adicionar novo tipo de observador = modificar esta classe
                // Viola Open/Closed Principle
            }
        }

        // Problema: Não há forma de remover observadores dinamicamente
        // Problema: Não suporta múltiplos observadores do mesmo tipo
    }

    public class Investor
    {
        public string Name { get; set; }
        public decimal AlertThreshold { get; set; }

        public Investor(string name, decimal alertThreshold)
        {
            Name = name;
            AlertThreshold = alertThreshold;
        }

        public void OnPriceChanged(string symbol, decimal price, decimal changePercent)
        {
            Console.WriteLine($"  → [Investidor {Name}] Notificado sobre {symbol}");
            
            if (Math.Abs(changePercent) >= AlertThreshold)
            {
                Console.WriteLine($"  → [Investidor {Name}] ⚠️ ALERTA! Mudança de {changePercent:+0.00;-0.00}% excedeu limite de {AlertThreshold}%");
            }
        }
    }

    public class MobileApp
    {
        public string UserId { get; set; }

        public MobileApp(string userId)
        {
            UserId = userId;
        }

        public void SendPushNotification(string symbol, decimal price, decimal changePercent)
        {
            Console.WriteLine($"  → [App Mobile {UserId}] 📱 Push: {symbol} agora em R$ {price:N2} ({changePercent:+0.00;-0.00}%)");
        }
    }

    public class TradingBot
    {
        public string BotName { get; set; }
        public decimal BuyThreshold { get; set; }
        public decimal SellThreshold { get; set; }

        public TradingBot(string botName, decimal buyThreshold, decimal sellThreshold)
        {
            BotName = botName;
            BuyThreshold = buyThreshold;
            SellThreshold = sellThreshold;
        }

        public void AnalyzeAndTrade(string symbol, decimal price, decimal changePercent)
        {
            Console.WriteLine($"  → [Bot {BotName}] 🤖 Analisando {symbol}...");
            
            if (changePercent <= -BuyThreshold)
            {
                Console.WriteLine($"  → [Bot {BotName}] 💰 COMPRANDO {symbol} por R$ {price:N2}");
            }
            else if (changePercent >= SellThreshold)
            {
                Console.WriteLine($"  → [Bot {BotName}] 💸 VENDENDO {symbol} por R$ {price:N2}");
            }
        }
    }

    // Alternativa problemática: Polling
    public class StockMonitor
    {
        private Stock _stock;
        private decimal _lastKnownPrice;

        public StockMonitor(Stock stock)
        {
            _stock = stock;
            _lastKnownPrice = stock.Price;
        }

        public void StartPolling()
        {
            // Problema: Polling constante desperdiça recursos
            while (true)
            {
                Thread.Sleep(1000); // Verifica a cada segundo
                
                if (_stock.Price != _lastKnownPrice)
                {
                    Console.WriteLine($"Mudança detectada por polling!");
                    _lastKnownPrice = _stock.Price;
                    // Como notificar múltiplos interessados?
                }
            }
        }

        // Problema: Latência (atraso de até 1 segundo)
        // Problema: Desperdício de CPU verificando constantemente
        // Problema: Não escala para milhares de ações
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Monitoramento de Ações ===");

            var petr4 = new Stock("PETR4", 35.50m);

            // Problema: Precisa registrar cada observador individualmente
            var investor1 = new Investor("João Silva", 3.0m);
            var investor2 = new Investor("Maria Santos", 5.0m);
            var mobileApp = new MobileApp("user123");
            var tradingBot = new TradingBot("AlgoTrader", 2.0m, 2.5m);

            petr4.RegisterInvestor1(investor1);
            petr4.RegisterInvestor2(investor2);
            petr4.RegisterMobileApp(mobileApp);
            petr4.RegisterTradingBot(tradingBot);

            // Simulando mudanças de preço
            Console.WriteLine("\n=== Movimentações do Mercado ===");
            
            petr4.UpdatePrice(36.20m); // +1.97%
            Thread.Sleep(500);
            
            petr4.UpdatePrice(37.50m); // +3.59%
            Thread.Sleep(500);
            
            petr4.UpdatePrice(35.00m); // -6.67%
            Thread.Sleep(500);

            // Problema: Como adicionar um terceiro investidor?
            // Precisaria adicionar _investor3 na classe Stock!
            
            // Problema: Como remover observadores?
            // Não há método de unregister!

            Console.WriteLine("\n=== PROBLEMAS ===");
            Console.WriteLine("✗ Acoplamento forte entre Stock e observadores específicos");
            Console.WriteLine("✗ Stock precisa conhecer cada tipo de observador");
            Console.WriteLine("✗ Adicionar novo observador = modificar classe Stock");
            Console.WriteLine("✗ Não suporta múltiplos observadores do mesmo tipo facilmente");
            Console.WriteLine("✗ Não há forma de remover observadores dinamicamente");
            Console.WriteLine("✗ Difícil adicionar novos tipos de notificação");
            Console.WriteLine("✗ Viola Open/Closed Principle");

            Console.WriteLine("\n=== Alternativa de Polling - Problemas ===");
            Console.WriteLine("✗ Latência (atraso entre mudança e detecção)");
            Console.WriteLine("✗ Desperdício de recursos (verificações constantes)");
            Console.WriteLine("✗ Não escala (milhares de ações × verificações por segundo)");
            Console.WriteLine("✗ Dificulta implementação de notificações em tempo real");

            // Perguntas para reflexão:
            // - Como desacoplar objeto observado dos observadores?
            // - Como notificar múltiplos objetos automaticamente?
            // - Como permitir subscrição/cancelamento dinâmico?
            // - Como criar dependência um-para-muitos desacoplada?
        }
    }
}
