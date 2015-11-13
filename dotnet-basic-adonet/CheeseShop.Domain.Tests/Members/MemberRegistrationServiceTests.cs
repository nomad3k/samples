using System.Linq;
using CheeseShop.Common;
using CheeseShop.Domain.Events;
using CheeseShop.Domain.Members;
using CheeseShop.Domain.Passwords;
using NUnit.Framework;

namespace CheeseShop.Domain.Tests.Members
{
    [TestFixture]
    public class MemberRegistrationServiceTests
    {
        private MockedBus _bus;
        private MockedMemberRepository _repository;
        private MemberRegistrationService _subject;
        private IPasswordFactory _passwordFactory;

        [SetUp]
        public void SetUp()
        {
            _bus = new MockedBus();
            _repository = new MockedMemberRepository();
            _passwordFactory = new PasswordFactory();
            _subject = new MemberRegistrationService(_bus,
                                                     _repository,
                                                     _passwordFactory);
        }

        [Test]
        public void Can_register_a_new_member()
        {
            DateUtil.FixForTesting();

            var member = _subject.Register("new@member.com", "New", "Member", "ANewPassword");

            Assert.That(_repository.Members, Has.Member(member), "Saved");
            var @event = _bus.Events.SingleOrDefault() as NewMemberRegisteredEvent;
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.Email, Is.EqualTo(member.Email));
            Assert.That(@event.Forename, Is.EqualTo(member.Forename));
            Assert.That(@event.Surname, Is.EqualTo(member.Surname));
            Assert.That(@event.ConfirmationCode, Is.Not.EqualTo(0));
            Assert.That(@event.RegisteredOn,  Is.EqualTo(DateUtil.Now()));
        }

        [Test]
        public void Cannot_register_an_existing_member()
        {
            DateUtil.FixForTesting();
            var passwordFactory = new PasswordFactory();
            var strategy = passwordFactory.Current;
            var existingMember = Member.Create("new@member.com", "Jenny", "Jones")
                                       .SetPassword(strategy, "TheUserHasAPassword");
            _repository.Save(existingMember);

            var member = _subject.Register("new@member.com",
                                           "Secondary",
                                           "Member",
                                           "ANewPassword");

            Assert.That(_repository.Members, Has.No.Member(member), "Saved");
            var @event = _bus.Events.SingleOrDefault() as ExistingMemberRegisteredEvent;
            Assert.That(@event, Is.Not.Null);
            Assert.That(@event.Email, Is.EqualTo("new@member.com"));
            Assert.That(@event.Forename, Is.EqualTo("Secondary"));
            Assert.That(@event.Surname, Is.EqualTo("Member"));
            Assert.That(@event.RealForename, Is.EqualTo("Jenny"));
            Assert.That(@event.RealSurname, Is.EqualTo("Jones"));
            Assert.That(@event.RegisteredOn, Is.EqualTo(DateUtil.Now()));
        }

        [Test]
        public void Can_confirm_an_existing_member()
        {
            var member = Member.Create("test@user.com", "Test", "User")
                               .Register();
            _repository.Save(member);

            var result = _subject.Confirm(member.Email, member.ConfirmationCode.Value);

            Assert.That(result, Is.True);
            Assert.That(_repository.Confirmed, Has.Member(member));
        }

        [Test]
        public void Cannot_confirm_an_existing_member_with_wrong_code()
        {
            var member = Member.Create("test@user.com", "Test", "User")
                               .Register();
            _repository.Save(member);

            var result = _subject.Confirm(member.Email, -1);

            Assert.That(result, Is.False);
            Assert.That(_repository.Confirmed, Has.No.Member(member));
        }

        [Test]
        public void Cannot_confirm_an_non_existing_member()
        {
            var result = _subject.Confirm("test@user.com", -1);

            Assert.That(result, Is.False);
            Assert.That(_repository.Confirmed, Is.Empty);
        }
    }
}