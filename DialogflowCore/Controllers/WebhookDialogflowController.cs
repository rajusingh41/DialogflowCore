using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DialogflowCore.Controllers
{
    [Route("api/webhookDialogflow")]
    [ApiController]
    public class WebhookDialogflowController : ControllerBase
    {
        private static readonly JsonParser jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

        private const string UserName = "Raju singh";

        [HttpPost]
        public async Task<IActionResult> GetWebhookResponse()
        {
            WebhookRequest request;
            using (var reader = new StreamReader(Request.Body))
            {
                request = jsonParser.Parse<WebhookRequest>(reader);
            }

            var actionType = request.QueryResult.Action;
            var parameters = request.QueryResult.Parameters;
            var response = new WebhookResponse();
            switch (actionType)
            {
                case "input.welcome":
                    response.FulfillmentText = $"Hi {UserName}, I am trip palnner agent how can help you.";
                    return Ok(response);
                case "input.flight":
                    var flightDate = parameters.Fields["date"].ToString();
                    var flyingFrom = parameters.Fields["flyingFrom"].ToString();
                    var flyingTo = parameters.Fields["flyingTo"].ToString();
                    response.FulfillmentText = $"Congrax your flight from {flyingFrom} to {flyingTo} booked for {flightDate}";
                    return Ok(response);
                case "input.hotal":
                    response.FulfillmentText = "your hotal has been booked";
                    return Ok(response);
                default:
                    response.FulfillmentText = "Sorry ask somting else";
                    return Ok(response);
            }
        }
    }
}