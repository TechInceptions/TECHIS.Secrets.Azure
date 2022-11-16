namespace TECHIS.Secrets
{
    public interface ISecretStore
    {
        string GetSecret (string key);
    }
}
