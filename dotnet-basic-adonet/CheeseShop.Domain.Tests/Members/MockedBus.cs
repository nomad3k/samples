using System.Collections.Generic;
using CheeseShop.Domain.Bus;
using CheeseShop.Domain.Events;

namespace CheeseShop.Domain.Tests.Members
{
    public class MockedBus : IBus
    {
        public IList<IDomainEvent> Events { get; private set; }

        public MockedBus()
        {
            Events = new List<IDomainEvent>();
        }

        public void Handle<TEvent>(TEvent @event) 
            where TEvent : IDomainEvent
        {
            Events.Add(@event);
        }
    }
}