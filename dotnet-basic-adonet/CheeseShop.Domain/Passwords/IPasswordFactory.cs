namespace CheeseShop.Domain.Passwords
{
    public interface IPasswordFactory
    {
        IPasswordStrategy Current { get; }
        IPasswordStrategy Get(int version);
    }
}