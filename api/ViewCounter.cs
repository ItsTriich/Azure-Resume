using System;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using System.Text.Json;

namespace My.Functions
{
    public static class ViewCounter
    {
        private static readonly string EndpointUri = Environment.GetEnvironmentVariable("COSMOSDB_ENDPOINT");
        private static readonly string PrimaryKey = Environment.GetEnvironmentVariable("COSMOSDB_PRIMARY_KEY");
        private static CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
        private static Database database = cosmosClient.GetDatabase("ViewCounterDB");
        private static Container container = database.GetContainer("Count");

        [Function("ViewCounter")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("ViewCounter");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                // Retrieve the current view count
                var response = await container.ReadItemAsync<dynamic>("viewCounter", new PartitionKey("viewCounter"));
                int currentCount = response.Resource.count;

                // Increment the view count
                currentCount++;

                // Update the view count in Cosmos DB
                var updatedItem = new { id = "viewCounter", count = currentCount };
                await container.UpsertItemAsync(updatedItem, new PartitionKey("viewCounter"));

                // Return the updated view count as JSON
                var responseMessage = req.CreateResponse(HttpStatusCode.OK);
                responseMessage.Headers.Add("Content-Type", "application/json");
                var jsonResponse = JsonSerializer.Serialize(new { count = currentCount });
                await responseMessage.WriteStringAsync(jsonResponse);

                return responseMessage;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred: {ex.Message}");

                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("An error occurred while processing your request.");
                return errorResponse;
            }
        }
    }
}
