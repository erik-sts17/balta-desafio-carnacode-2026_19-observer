using Observer.Application.Services;
using Observer.Domain.Entities;

var service = new StockService();
service.Subscribe(new InvestorService());
service.Subscribe(new SmsService());
service.UpdatePrice(new Stock("CDB", 200, DateTime.Now));