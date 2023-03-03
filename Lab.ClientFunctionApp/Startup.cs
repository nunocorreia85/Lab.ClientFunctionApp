using System;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(Lab.ClientFunctionApp.Startup))]
namespace Lab.ClientFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var cs = Environment.GetEnvironmentVariable("ConnectionString");
            if(cs == null)
            {
                builder.ConfigurationBuilder.AddAzureAppConfiguration(options => 
                    options.Connect(new Uri("https://testappconfiguraitonnuno.azconfig.io"), 
                        new DefaultAzureCredential(new DefaultAzureCredentialOptions
                        {
                            ManagedIdentityClientId = "c04c17c3-c789-4aee-95a2-56cb8d6427f0"
                        })));
            }
            else
            {
                builder.ConfigurationBuilder.AddAzureAppConfiguration(cs);
            }
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
        }
    }
}
