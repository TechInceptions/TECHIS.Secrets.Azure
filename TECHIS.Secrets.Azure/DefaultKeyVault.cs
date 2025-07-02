using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Text;
using Azure.Core;

namespace TECHIS.Secrets
{

    public class DefaultKeyVault : ISecretStore
    {
        private SecretClient _client;
        public string VaultUri { get; }
        public SecretClientOptions Options { get; }

        public DefaultKeyVault(string vaultUri):this(vaultUri,null)
        { 
        }

        public DefaultKeyVault(string vaultUri, bool preferManagedIdentity) 
            : this(vaultUri, 
                  preferManagedIdentity? new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeWorkloadIdentityCredential=true, ExcludeEnvironmentCredential=true }) : null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vaultUri">You key vault uri like: https://<your-unique-key-vault-name>.vault.azure.net/</param>
        /// <exception cref="ArgumentException"></exception>
        public DefaultKeyVault(string vaultUri, TokenCredential? tokenCredential)
        {
            if (string.IsNullOrWhiteSpace(vaultUri))
            {
                throw new ArgumentException($"'{nameof(vaultUri)}' cannot be null or whitespace.", nameof(vaultUri));
            }

            VaultUri = vaultUri;

            Options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };

            _client = new SecretClient(new Uri(vaultUri), tokenCredential?? new DefaultAzureCredential(), Options);
        }


        public string GetSecret(string key)
        {
            KeyVaultSecret secret = _client.GetSecret(key);

            return secret.Value;

        }
    }
}
