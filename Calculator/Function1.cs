using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Calculator
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string numX = req.Query["x"];
            string numY = req.Query["y"];
            string operand = req.Query["Operator"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            numX = numX ?? data?.x;
            numY = numY ?? data?.y;
            operand = operand ?? data?.Operator;

            float x = float.Parse(numX);
            float y = float.Parse(numY);

            string responseMessage = "";

            if (y == 0)
            {
                responseMessage = "Cannot divide by 0";
                return new OkObjectResult(responseMessage);
            }

            switch (operand)
            {
                case "+":
                    responseMessage = (x + y).ToString();
                    break;
                case "-":
                    responseMessage = (x - y).ToString();
                    break;
                case "*":
                    responseMessage = (x*y).ToString();
                    break;
                case "/":
                    responseMessage = (x / y).ToString();
                    break;
                case "%":
                    responseMessage = (x % y).ToString();
                    break;
                default:
                    responseMessage = "Incorrect operator assignment";
                    break;
            }

            
            return new OkObjectResult(responseMessage);
        }
    }
}
