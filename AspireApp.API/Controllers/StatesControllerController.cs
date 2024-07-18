using AspireApp.ServiceDefaults.Models;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace AspireApp.API.Controllers;

[Route("api/states")]
[ApiController]
public class StatesControllerController(
    DaprClient daprClient,
    ILogger<StatesControllerController> logger) : ControllerBase
{
    [HttpPost("save")]
    public async Task<ActionResult<Product>> Save([FromBody] Product product)
    {
        logger.LogInformation("Saving product: {Product}", product);

        await daprClient.SaveStateAsync(
            "statestore",
            product.Id.ToString(),
            product);

        return Ok(product);
    }

    [HttpGet("read")]
    public async Task<ActionResult<Product>> Read([FromQuery] int id)
    {
        Product? product = await daprClient.GetStateAsync<Product?>(
            "statestore",
            id.ToString());

        if (product == null)
        {
            return NotFound();
        }

        logger.LogInformation("Reading product with id: {Id} - {Product}", id, product);
        return Ok(product);
    }
}
