using System.Security.Cryptography;

namespace CheeseShop.Domain.Passwords
{
    public class Rfc2898PasswordStrategy : IPasswordStrategy
    {
        public const int VersionNumber = 1;
        public const int DefaultRounds = 300;

        public int? DefaultParameter
        {
            get { return DefaultRounds; }
        }

        public int Version
        {
            get { return VersionNumber;  }
        }


        public byte[] Generate(byte[] password, byte[] salt, int? param = null)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, param ?? DefaultRounds))
            {
                return rfc2898.GetBytes(32);
            }
        }
    }
}