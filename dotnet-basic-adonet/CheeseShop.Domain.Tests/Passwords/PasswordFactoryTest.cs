using CheeseShop.Domain.Passwords;
using NUnit.Framework;

namespace CheeseShop.Domain.Tests.Passwords
{
    [TestFixture]
    public class PasswordFactoryTest
    {
        [Test]
        public void PasswordFactory_always_has_a_current_strategy_which_is_non_null()
        {
            var subject = new PasswordFactory();
            Assert.That(subject.Current, Is.Not.Null);
        }
    }
}