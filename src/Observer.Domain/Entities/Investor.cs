namespace Observer.Domain.Entities
{
    public class Investor(string name, decimal alertThreshold)
    {
        public string Name { get; set; } = name;
        public decimal AlertThreshold { get; set; } = alertThreshold;
    }
}