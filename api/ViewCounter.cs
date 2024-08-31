using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace My.Functions
{
    public static class ViewCounter
    {
        private static readonly string EndpointUri = "https://cosmodb-232.documents.azure.com/";
        private static readonly string PrimaryKey = "f7Tt6So2iwwZxCfenZ4WGlDdtceDvl27DBjOY4cM0L09Vs3mVmD46OFhio6EY63mzWwXRkLx9wFYACDbPDHRTA==";
        private static CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
        private static Database database = cosmosClient.GetDatabase("ViewCounterDB");
        private static Container container = database.GetContainer("Count");

        [Function("ViewCounter")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("ViewCounter");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            // Retrieve the current view count
            var response = await container.ReadItemAsync<dynamic>("viewCounter", new PartitionKey("viewCounter"));
            int currentCount = response.Resource.count;

            // Increment the view count
            currentCount++;

            // Update the view count in Cosmos DB
            var updatedItem = new { id = "viewCounter", count = currentCount };
            await container.UpsertItemAsync(updatedItem, new PartitionKey("viewCounter"));

            // Return the updated view count
            var responseMessage = req.CreateResponse(HttpStatusCode.OK);
            await responseMessage.WriteStringAsync($"View count: {currentCount}");

            return responseMessage;
        }
    }
}
