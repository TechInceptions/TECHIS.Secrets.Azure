namespace TECHIS.Secrets
{
    public class DefaultKeyVaultFactory : ISecretStoreFactory
    {
        public ISecretStore GetSecretStore(string storeUri)
        {
            return new DefaultKeyVault(storeUri);
        }
    }
}
