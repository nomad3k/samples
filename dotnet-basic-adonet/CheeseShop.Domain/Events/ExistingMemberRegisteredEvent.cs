using System;
using CheeseShop.Common;
using CheeseShop.Domain.Bus;
using CheeseShop.Domain.Members;

namespace CheeseShop.Domain.Events
{
    public class ExistingMemberRegisteredEvent : IDomainEvent
    {
        public string Email { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }

        public string RealForename { get; set; }
        public string RealSurname { get; set; }
        public DateTime RegisteredOn { get; set; }

        public static ExistingMemberRegisteredEvent Create(Member existing,
                                                           Member member)
        {
            return new ExistingMemberRegisteredEvent
                {
                    Email = existing.Email,
                    Forename = member.Forename,
                    Surname = member.Surname,
                    RealForename = existing.Forename,
                    RealSurname = existing.Surname,
                    RegisteredOn = DateUtil.Now()
                };
        }
    }
}