using CheeseShop.Common;
using CheeseShop.Domain.Members;
using CheeseShop.Domain.Passwords;
using NUnit.Framework;

namespace CheeseShop.Domain.Tests.Members
{
    [TestFixture]
    public class MemberTests
    {
        [TestCase("john@blah.com", "John", "Smith")]
        [TestCase("dave@what.net", "Dave", "Davis")]
        public void Can_create_member(string email,
                                      string forename,
                                      string surname)
        {
            var member = Member.Create(email, forename, surname);

            Assert.That(member, Is.Not.Null);
            Assert.That(member.Email, Is.EqualTo(email));
            Assert.That(member.Forename, Is.EqualTo(forename));
            Assert.That(member.Surname, Is.EqualTo(surname));
        }

        [Test]
        public void Can_set_member_password()
        {
            const string password = "TheCurrentPassword";
            var passwordFactory = new PasswordFactory();
            var strategy = passwordFactory.Current;

            var member = Member.Create("test@thecheeseshop.com",
                                       "Test",
                                       "User")
                               .SetPassword(strategy, password);
            var valid = member.ValidatePassword(passwordFactory, password);

            Assert.That(member.Version, Is.EqualTo(strategy.Version), "Version");
            Assert.That(member.Parameter, Is.EqualTo(strategy.DefaultParameter), "Parameter");
            Assert.That(member.Salt, Is.Not.Null, "Salt");
            Assert.That(member.Password, Is.Not.Null, "Password");
            Assert.That(valid, Is.True, "Validate");
        }

        [Test]
        public void Can_change_member_password()
        {
            const string oldPassword = "TheOldPassword";
            const string newPassword = "TheNewPassword";
            var passwordFactory = new PasswordFactory();
            var strategy = passwordFactory.Current;

            var member = Member.Create("test@thecheeseshop.com",
                                       "Test",
                                       "User")
                               .SetPassword(strategy, oldPassword)
                               .SetPassword(strategy, newPassword);

            var oldValid = member.ValidatePassword(passwordFactory, oldPassword);
            var newValid = member.ValidatePassword(passwordFactory, newPassword);
            Assert.That(oldValid, Is.False, "Old Password");
            Assert.That(newValid, Is.True, "New Password");
        }

        [Test]
        public void Can_register_member()
        {
            var member = Member.Create("test@user.com", "Test", "User")
                               .Register();
            Assert.That(member, Is.Not.Null);
            Assert.That(member.ConfirmationCode, Is.Not.Null);
            Assert.That(member.ConfirmationDate, Is.Null);
        }

        [Test]
        public void Can_confirm_member_with_valid_code()
        {
            var member = Member.Create("test@user.com", "Test", "User")
                               .Register();
            var confirmed = member.Confirm(member.ConfirmationCode.Value);
            Assert.That(confirmed, Is.True, "Confirmed");
            Assert.That(member.ConfirmationDate, Is.EqualTo(DateUtil.Now()), "Confirmation Date");
        }

        [Test]
        public void Cannot_confirm_member_without_valid_code()
        {
            DateUtil.FixForTesting();
            var member = Member.Create("test@user.com", "Test", "User")
                               .Register();
            var confirmed = member.Confirm(-1);
            Assert.That(confirmed, Is.False, "Confirmed");
            Assert.That(member.ConfirmationDate, Is.Null, "Confirmation Date");
        }
    }
}
