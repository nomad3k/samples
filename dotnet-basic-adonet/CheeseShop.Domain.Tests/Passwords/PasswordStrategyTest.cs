using System.Text;
using CheeseShop.Domain.Passwords;
using NUnit.Framework;

namespace CheeseShop.Domain.Tests.Passwords
{
    /// <summary>
    /// This test ensures the Liskov's substitution principal is adhered to.
    /// </summary>
    /// <typeparam name="TPasswordStrategy"></typeparam>
    [TestFixture]
    public abstract class PasswordStrategyTest<TPasswordStrategy>
        where TPasswordStrategy : IPasswordStrategy, new()
    {
        [Test]
        public void Password_strategy_is_deterministic()
        {
            const string passwordStr = "Thi$IsMyP@ssw0rd";
            var salt = PasswordFactory.GenerateSalt();
            var password = Encoding.UTF8.GetBytes(passwordStr);

            var resultA = new TPasswordStrategy().Generate(password, salt);
            var resultB = new TPasswordStrategy().Generate(password, salt);

            Assert.That(resultA, Is.EqualTo(resultB));
        }

        [Test]
        public void Strategy_is_registed_with_the_factory()
        {
            var version = new TPasswordStrategy().Version;
            var subject = new PasswordFactory();
            var result = subject.Get(version);

            Assert.That(result, Is.InstanceOf<Rfc2898PasswordStrategy>());
        }
    }
}
