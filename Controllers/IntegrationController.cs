using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace qpssbpmock.Controllers;
//------------------------------------------------------------------------------
[ApiController]
[Route("c2c/[controller]")]
public class IntegrationController : ControllerBase
{
    private static readonly string[] Names = new[]
    {
        "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Heidi"
    };
    private static readonly string[] Surnames = new[]
    {
        "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson"
    };
    private static readonly string[] Patronymics = new[]
    {
        "Patrinymic1", "Patrinymic2", "Patrinymic3", "Patrinymic4", "Patrinymic0", "Patrinymic5", "Patrinymic6", "Patrinymic7"
    };
    private static readonly string[] Adresses = new[]
    {
        "Adress1", "Adress2", "Adress3", "Adress4", "Adress5", "Adress6", "Adress7", "Adress8"
    };

    private readonly int mockResponseTime;
    private readonly ILogger<IntegrationController> _logger;

    public IntegrationController(ILogger<IntegrationController> logger, IConfiguration configuration)
    {
        _logger = logger;
        // Get value from appsettings.json.
        mockResponseTime = configuration.GetValue<int>("MockResponseTime");
    }
    //----------------------------------------------------------------------------
    [Consumes("application/json")]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("CheckReceiver")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpPost()]
    public JsonResult PostCheckReceiver([FromBody] JsonElement body)
    {
        // Extracting receiver phone number.
        string resPhNum = body.GetProperty("ReceiverPhoneNumber").GetString();
        resPhNum = new string(resPhNum.Substring(1));

        // Extracting target bank ID.
        string tgBankId = body.GetProperty("TargetBankId").GetString();

        // Shuffle data for the response.
        Random rnd = new();
        string receName = Names[rnd.Next(Names.Length)];
        string receSurname = Surnames[rnd.Next(Surnames.Length)];
        string recePatronymic = Patronymics[rnd.Next(Patronymics.Length)];
        string receAdress = Adresses[rnd.Next(Adresses.Length)];

        // Create response.
        var jsonData = new
        {
            data = new
            {
            receiverPhoneNumber = resPhNum,
            receiverBankId = tgBankId,
            receiverBankName = "BankName",
            receiverBik = "0123456789",
            receiverInn = "0123456789012",
            receiverFullName = $"{receSurname}|{receName}|{recePatronymic}",
            receiverAddress = receAdress,
            pam = $"{receName} {recePatronymic} {receSurname.Substring(0, 1)}.",
            state = 0,
            businessMessageId = Guid.NewGuid().ToString().Replace("-", ""),
            Scoring = new
            {
                ipsScoring = (string)null,
                senderScoring = "22200F1000DDDDDD",
                receiverScoring = "FFFFFDDDDDDDDDD"
            },
            message = (string)null,
            systemMessage = (string)null,
            code = 200
            }
        };

        // Serializing options.
        var serializingOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        // Response JSON. Unsafe but displays cyrillic symbols correctly.
        var responseJSON = new JsonResult(jsonData, serializingOptions);

        // Log only in Information level.
        _logger.LogInformation($"Request: {body}");
        _logger.LogInformation($"Response: {JsonSerializer.Serialize(responseJSON, serializingOptions)}");

        Task.Delay(mockResponseTime).Wait();
        return responseJSON;
    }
}