namespace TECHIS.Secrets
{
    public interface ISecretStoreFactory
    {
        ISecretStore GetSecretStore (string storeUri);
    }
}
