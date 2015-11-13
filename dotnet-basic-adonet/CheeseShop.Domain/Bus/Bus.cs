using Castle.Windsor;

namespace CheeseShop.Domain.Bus
{
    public class Bus : IBus
    {
        private readonly IWindsorContainer _container;

        public Bus(IWindsorContainer container)
        {
            _container = container;
        }

        public void Handle<TEvent>(TEvent @event) 
            where TEvent : IDomainEvent
        {
            var handlers = _container.ResolveAll<IDomainEventHandler<TEvent>>();
            foreach (var handler in handlers)
                handler.Handle(@event);
        }
    }
}