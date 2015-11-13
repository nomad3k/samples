using System;
using CheeseShop.Common;
using CheeseShop.Domain.Bus;
using CheeseShop.Domain.Members;

namespace CheeseShop.Domain.Events
{
    public class NewMemberRegisteredEvent : IDomainEvent
    {
        public string Email { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime RegisteredOn { get; set; }
        public int ConfirmationCode { get; set; }

        public static NewMemberRegisteredEvent Create(Member member)
        {
            return new NewMemberRegisteredEvent
                {
                    Email = member.Email,
                    Forename = member.Forename,
                    Surname = member.Surname,
                    ConfirmationCode = member.ConfirmationCode.Value,
                    RegisteredOn = DateUtil.Now()
                };
        }
    }
}