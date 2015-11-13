using System;
using System.Data;
using System.Linq;
using CheeseShop.Common;

namespace CheeseShop.Domain.Members
{
    public interface IMemberRepository
    {
        Member Get(string email);
        void Save(Member member);
        void Confirm(Member member);
    }

    public class MemberRepository : IMemberRepository
    {
        private readonly IDbConnection _connection;

        public MemberRepository(IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            _connection = connection;
        }

        public Member Get(string email)
        {
            return _connection.Query(@"
                SELECT Email, Forename, Surname, Parameter, Salt, Password, ConfirmationCode, ConfirmationDate 
                FROM [security].[Member] 
                WHERE Email = @Email",
                                     new {email})
                              .Select(row => new Member
                                  {
                                      Email = row[0] as string,
                                      Forename = row[1] as string,
                                      Surname = row[2] as string,
                                      Parameter = row[3] as int?,
                                      Salt = row[4] as byte[],
                                      Password = row[5] as byte[],
                                      ConfirmationCode = row[6] as int?,
                                      ConfirmationDate = row[7] as DateTime?
                                  })
                              .FirstOrDefault();
        }

        public void Save(Member member)
        {
            _connection.Execute(@"
                INSERT INTO [security].[Member]
                (Email, Forename, Surname, Version, Parameter, Salt, Password, ConfirmationCode, ConfirmationDate)
                VALUES(@Email, @Forename, @Surname, @Version, @Parameter, @Salt, @Password, @ConfirmationCode, @ConfirmationDate)",
                                member);
        }

        public void Confirm(Member member)
        {
            _connection.Execute(@"
                UPDATE [security].[Member]
                SET ConfirmationDate = @ConfirmationDate
                WHERE Email = @Email
                AND ConfirmationDate IS NULL",
                                new
                                    {
                                        member.Email,
                                        member.ConfirmationDate
                                    });
        }
    }
}