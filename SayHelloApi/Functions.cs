using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace NewSig.Examples
{
    public static class Functions
    {
        [FunctionName("SayHelloEnglish")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IActionResult> SayHelloEnglish(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "english/hello")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var name = data?.name;

            return new OkObjectResult($"Hello, {name}");
        }

        [FunctionName("SayHelloGerman")]
        public static async Task<IActionResult> SayHelloGerman(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "german/hello")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var name = data?.name;

            return new OkObjectResult($"Hallo, {name}");
        }

        [FunctionName("SayHelloKorean")]
        public static async Task<IActionResult> SayHelloKorean(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "korean/hello")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var name = data?.name;

            return new OkObjectResult($"여보세요, {name}");
        }

        [FunctionName("SayHelloChinese")]
        public static async Task<IActionResult> SayHelloChinese(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "chinese/hello")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var name = data?.name;

            return new OkObjectResult($"你好, {name}");
        }

        [FunctionName("SayHelloFrench")]
        public static async Task<IActionResult> SayHelloFrench(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "french/hello")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var name = data?.name;

            return new OkObjectResult($"Bonjour, {name}");
        }

        [FunctionName("SayHelloJapanese")]
        public static async Task<IActionResult> SayHelloJapanese(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "japanese/hello")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            var name = data?.name;

            return new OkObjectResult($"こんにちは, {name}");
        }
    }
}
