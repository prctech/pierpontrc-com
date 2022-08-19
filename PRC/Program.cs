using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PRC.Extensions;

namespace PRC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    /*if (context.HostingEnvironment.IsProduction())
                    {
                        config.ConfigureKeyVault();
                        
                    
                        /Exclude below
                        var builtConfig = config.Build();
                        var secretClient = new SecretClient(
                            new Uri($"https://{builtConfig["KeyVaultName"]}.vault.azure.net/"),
                            new DefaultAzureCredential());
                        config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
                    }
                    else
                        config.WriteConfigurationSources();*/
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
