using System;
using System.Collections.Generic;
using System.Linq;
using CheeseShop.Domain.Members;

namespace CheeseShop.Domain.Tests.Members
{
    public class MockedMemberRepository : IMemberRepository
    {
        public IList<Member> Members { get; private set; }
        public IList<Member> Confirmed { get; private set; }

        public MockedMemberRepository()
        {
            Members = new List<Member>();
            Confirmed = new List<Member>();
        }

        public Member Get(string email)
        {
            return Members.FirstOrDefault(x => string.Equals(x.Email, email, StringComparison.InvariantCultureIgnoreCase));
        }

        public void Save(Member member)
        {
            Members.Add(member);
        }

        public void Confirm(Member member)
        {
            Confirmed.Add(member);
        }
    }
}