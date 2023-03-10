using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Lab.ClientFunctionApp
{
    public class CallServiceHttpTrigger
    {
        private readonly IConfiguration _configuration;
        public CallServiceHttpTrigger(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [FunctionName("CallServiceHttpTrigger")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var appConfiguration = new AppConfiguration();
            _configuration.Bind(appConfiguration);
            var serviceEndpoint = appConfiguration.ServiceFunctionAddress ??
                                  throw new ArgumentNullException(nameof(appConfiguration.ServiceFunctionAddress));
            log.LogInformation("serviceEndpoint: {ServiceEndpoint}", serviceEndpoint);
            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            
            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");

            
        }
    }
}
