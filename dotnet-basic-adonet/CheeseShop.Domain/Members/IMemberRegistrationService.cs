namespace CheeseShop.Domain.Members
{
    public interface IMemberRegistrationService
    {
        Member Register(string email,
                        string forename,
                        string surname,
                        string password);

        bool Confirm(string email,
                     int confirmationCode);
    }
}