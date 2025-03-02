using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace qpssbpmock.Controllers;
//------------------------------------------------------------------------------
[ApiController]
[Route("c2c/[controller]")]
public class FtsController : ControllerBase
{
    private readonly int mockResponseTime;
    private readonly ILogger<FtsController> _logger;

    public FtsController(ILogger<FtsController> logger, IConfiguration configuration)
    {
        _logger = logger;
        // Get value from appsettings.json.
        mockResponseTime = configuration.GetValue<int>("MockResponseTime");
    }
    //----------------------------------------------------------------------------
    // Handler for POST /c2c/fts/bankLists
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("bankList")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpPost()]
    public JsonResult PostBankList([FromBody] JsonElement body)
    {
        var jsonData = new[]
        {
            new {
                bankName = "Bank1",
                bankBic = "1234567890",
                memberId = "100000000001",
                supportedScenario = 3
            },
            new {
                bankName = "Bank2",
                bankBic = "1234567890",
                memberId = "100000000002",
                supportedScenario = 3
            },
            new {
                bankName = "Bank3",
                bankBic = "1234567890",
                memberId = "100000000003",
                supportedScenario = 3
            },
            new {
                bankName = "Bank4",
                bankBic = "1234567890",
                memberId = "100000000004",
                supportedScenario = 3
            },
            new {
                bankName = "Bank5",
                bankBic = "1234567890",
                memberId = "100000000005",
                supportedScenario = 3
            },

        };

        var serializingOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        var responseJSON = new JsonResult(jsonData, serializingOptions);

        _logger.LogInformation($"Request: {body}");
        _logger.LogInformation($"Rsponse: {JsonSerializer.Serialize(responseJSON, serializingOptions)}");

        Task.Delay(mockResponseTime).Wait();
        return responseJSON;
    }

}