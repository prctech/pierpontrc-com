using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;

namespace PRC.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static void ConfigureKeyVault(this IConfigurationBuilder config)
        {
            string? DefaultConnection = Environment.GetEnvironmentVariable("DefaultConnection");
            string? DevelConnection = Environment.GetEnvironmentVariable("DevelConnection");
            string? TinyMCEAPIKey = Environment.GetEnvironmentVariable("KEYVAULT_ENDPOINT");

            if (DefaultConnection is null)
                throw new InvalidOperationException("Store the Key Vault endpoint in a DefaultConnection environment variable.");

            if (DevelConnection is null)
                throw new InvalidOperationException("Store the Key Vault endpoint in a DevelConnection environment variable.");

            if (TinyMCEAPIKey is null)
                throw new InvalidOperationException("Store the Key Vault endpoint in a TinyMCEAPIKey environment variable.");

            var defaultClient = new SecretClient(new System.Uri(DefaultConnection), new DefaultAzureCredential());
            config.AddAzureKeyVault(defaultClient, new KeyVaultSecretManager());

            var develClient = new SecretClient(new System.Uri(DevelConnection), new DefaultAzureCredential());
            config.AddAzureKeyVault(defaultClient, new KeyVaultSecretManager());

            var mceClient = new SecretClient(new System.Uri(TinyMCEAPIKey), new DefaultAzureCredential());
            config.AddAzureKeyVault(defaultClient, new KeyVaultSecretManager());
        }

        public static void WriteConfigurationSources(this IConfigurationBuilder config)
        {
            Console.WriteLine("Configuration sources\n=====================");
            foreach (var source in config.Sources)
            {
                if (source is JsonConfigurationSource jsonSource)
                    Console.WriteLine($"{source}: {jsonSource.Path}");
                else
                    Console.WriteLine(source.ToString());
            }
            Console.WriteLine("=====================\n");
        }
    }
}
