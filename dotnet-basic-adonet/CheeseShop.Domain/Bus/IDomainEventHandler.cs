namespace CheeseShop.Domain.Bus
{
    public interface IDomainEventHandler<TEvent>
        where TEvent : IDomainEvent
    {
        void Handle(TEvent e);
    }
}