namespace PlaystationWishlist.Core.Interfaces
{
    public interface IKeyVaultService
    {
        string GetSecret(string name);
    }
}