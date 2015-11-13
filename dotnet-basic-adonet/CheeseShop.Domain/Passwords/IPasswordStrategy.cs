namespace CheeseShop.Domain.Passwords
{
    public interface IPasswordStrategy
    {
        int? DefaultParameter { get; }
        int Version { get; }
        byte[] Generate(byte[] password, byte[] salt, int? param = null);
    }
}