using System;
using CheeseShop.Common;
using CheeseShop.Domain.Bus;
using CheeseShop.Domain.Events;
using CheeseShop.Domain.Passwords;

namespace CheeseShop.Domain.Members
{
    public class MemberRegistrationService : IMemberRegistrationService
    {
        private readonly IBus _bus;
        private readonly IMemberRepository _repository;
        private readonly IPasswordFactory _passwordFactory;

        public MemberRegistrationService(IBus bus,
                                         IMemberRepository repository,
                                         IPasswordFactory passwordFactory)
        {
            _bus = bus;
            _repository = repository;
            _passwordFactory = passwordFactory;
        }

        public Member Register(string email,
                               string forename,
                               string surname,
                               string password)
        {
            var member = Member.Create(email, forename, surname)
                               .SetPassword(_passwordFactory.Current, password);
            var existing = _repository.Get(email);

            if (existing != null)
            {
                _bus.Handle(ExistingMemberRegisteredEvent.Create(existing, member));
            }
            else
            {
                member.Register();
                _repository.Save(member);
                _bus.Handle(NewMemberRegisteredEvent.Create(member));
            }

            return member;
        }

        public bool Confirm(string email,
                            int confirmationCode)
        {
            var member = _repository.Get(email);
            if (member == null || !member.Confirm(confirmationCode))
                return false;

            _repository.Confirm(member);
            return true;
        }
    }
}