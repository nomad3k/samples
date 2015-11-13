using CheeseShop.Domain.Events;

namespace CheeseShop.Domain.Bus
{
    public interface IBus
    {
        void Handle<TEvent>(TEvent @event)
            where TEvent : IDomainEvent;
    }
}