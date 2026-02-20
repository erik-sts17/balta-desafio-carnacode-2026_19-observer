namespace Observer.Domain.Abstractions
{
    public interface IStockSubject
    {
        public void Subscribe(IStockObserver observer);
        public void Unsubscribe(IStockObserver observer);
    }
}