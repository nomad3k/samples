using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace CheeseShop.Domain.Passwords
{
    public class PasswordFactory : IPasswordFactory
    {
        private readonly IDictionary<int, Func<IPasswordStrategy>> _versions;

        public IPasswordStrategy Current
        {
            get { return Get(Rfc2898PasswordStrategy.VersionNumber); }
        }

        public PasswordFactory()
        {
            _versions = new Dictionary<int, Func<IPasswordStrategy>>
            {
                {Rfc2898PasswordStrategy.VersionNumber, () => new Rfc2898PasswordStrategy() }
            };
        }

        public static byte[] GenerateSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[32];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public IPasswordStrategy Get(int version)
        {
            return _versions[version]();
        }
    }
}