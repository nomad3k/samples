using System;
using System.Text;
using CheeseShop.Common;
using CheeseShop.Domain.Passwords;

namespace CheeseShop.Domain.Members
{
    public class Member
    {
        public string Email { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        
        public int Version { get; set; }
        public int? Parameter { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Password { get; set; }

        public int? ConfirmationCode { get; set; }
        public DateTime? ConfirmationDate { get; set; }

        public Member SetPassword(IPasswordStrategy passwordStrategy, 
                                  string password, 
                                  int? parameter = null)
        {
            if (passwordStrategy == null) throw new ArgumentNullException("passwordStrategy");
            if (password == null) throw new ArgumentNullException("password");

            Version = passwordStrategy.Version;
            Parameter = parameter ?? passwordStrategy.DefaultParameter;
            var encoded = Encoding.UTF8.GetBytes(password);
            if (Salt == null) 
                Salt = PasswordFactory.GenerateSalt();
            Password = passwordStrategy.Generate(encoded, Salt, Parameter);
            return this;
        }

        public bool ValidatePassword(IPasswordFactory passwordFactory, string password)
        {
            if (passwordFactory == null) throw new ArgumentNullException("passwordFactory");
            if (password == null) throw new ArgumentNullException("password");

            var strategy = passwordFactory.Get(Version);
            var encoded = Encoding.UTF8.GetBytes(password);
            var hashed = strategy.Generate(encoded, Salt, Parameter);

            return CompareWithSameExecutionTime(hashed);
        }

        // Byte array comparisons that terminate more quickly when they fail
        // are attack vectors.  Therefore always take the same amount of time.
        private bool CompareWithSameExecutionTime(byte[] compareTo)
        {
            var result = true;
            for (var i = 0; i < Password.Length; i++)
                result &= Password[i] == compareTo[i];
            return result;
        }

        // I use a static method rather than a constructor as it leads to neatly
        // formatted fluent style code and the method can be passed into linq
        // statements (if the signature matches). e.g.
        //
        // var member = Member.Create("email", "forename", "surname")
        //                    .SetPassword(strategy, "pass");
        //
        // var members = db.LoadAllMembers()
        //                 .Select(Member.Create)
        //                 .ToArray();

        public static Member Create(string email,
                                    string forename,
                                    string surname)
        {
            return new Member
                {
                    Email = email,
                    Forename = forename,
                    Surname = surname
                };
        }

        public Member Register()
        {
            ConfirmationCode = new Random().Next(1000000);
            ConfirmationDate = null;
            return this;
        }

        public bool Confirm(int confirmationCode)
        {
            if (ConfirmationCode != confirmationCode)
                return false;

            ConfirmationDate = DateUtil.Now();
            return true;
        }
    }
}