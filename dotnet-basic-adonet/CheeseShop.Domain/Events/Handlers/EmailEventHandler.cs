using System;
using System.IO;
using System.Web;
using CheeseShop.Common;
using CheeseShop.Domain.Bus;
using RazorEngine;

namespace CheeseShop.Domain.Events
{
    public class EmailEventHandler
        : IDomainEventHandler<ExistingMemberRegisteredEvent>
        , IDomainEventHandler<NewMemberRegisteredEvent> 
    {
        private readonly IFluentEmailClient _emailClient;

        public EmailEventHandler(IFluentEmailClient emailClient)
        {
            if (emailClient == null) throw new ArgumentNullException("emailClient");
            _emailClient = emailClient;
        }

        private string Render<TEvent>(TEvent @event)
        {
            var resource = string.Format("CheeseShop.Domain.Events.Handlers.Messages.{0}.cshtml", typeof(TEvent).Name);
            using (var stream = GetType().Assembly.GetManifestResourceStream(resource))
            using (var reader = new StreamReader(stream))
            {
                var view = reader.ReadToEnd();
                return Razor.Parse(view, @event);
            }
        }

        private IFluentEmail Email(string author, string fromAddress)
        {
            return _emailClient.Prepare()
#if RELEASE
                               .Exclude(author)
#endif
                               .ReplyTo(author)
                               .From(fromAddress);
        }

        public void Handle(NewMemberRegisteredEvent e)
        {
            Email("noreply@cheeseshop.com", "registration@cheeseshop.com")
                .To(e.Email)
                .Subject("Welcome to The Cheese Shop {0}", e.Forename)
                .Body(() => Render(e))
                .Send();
        }

        public void Handle(ExistingMemberRegisteredEvent e)
        {
            Email("noreply@cheeseshop.com", "registration@cheeseshop.com")
                .To(e.Email)
                .Subject("Message from The Cheese Shop")
                .Body(() => Render(e))
                .Send();
        }
    }
}
