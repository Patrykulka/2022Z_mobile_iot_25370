using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("GetPeople")]
        public static async Task<IActionResult> GetPeople(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "People")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try {
                string connectionString = Environment.GetEnvironmentVariable("PersonDb");
                log.LogInformation(connectionString);
                var db = new Context(connectionString);
                var people = db.GetPeople();
                return new JsonResult(people);
            } catch (Exception ex) {
                log.LogError(ex, ex.Message);
                return new JsonResult(ex);
            }
        }

        [FunctionName("AddPerson")]
        async public static Task<IActionResult> AddPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Person")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try {
                PersonInsert person = await JsonSerializer.DeserializeAsync<PersonInsert>(req.Body);
                string connectionString = Environment.GetEnvironmentVariable("PersonDb");
                log.LogInformation(connectionString);
                var db = new Context(connectionString);
                db.InsertPerson(person);
                return new OkResult();
            } catch (Exception ex) {
                log.LogError(ex, ex.Message);
                return new JsonResult(ex);
            }
        }

        [FunctionName("GetById")]
        public static async Task<IActionResult> GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Person/{id}")] HttpRequest req,
            ILogger log, int id)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try {
                string connectionString = Environment.GetEnvironmentVariable("PersonDb");
                log.LogInformation(connectionString);
                var db = new Context(connectionString);
                var people = db.GetPerson(id);
                return new JsonResult(people);
            } catch (Exception ex) {
                log.LogError(ex, ex.Message);
                return new JsonResult(ex);
            }
        }

        [FunctionName("DeletePerson")]
        public static async Task<IActionResult> DeletePerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "Person/{id}")] HttpRequest req,
            ILogger log, int id)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try {
                string connectionString = Environment.GetEnvironmentVariable("PersonDb");
                log.LogInformation(connectionString);
                var db = new Context(connectionString);
                db.DeletePerson(id);
                return new OkResult();
            } catch (Exception ex) {
                log.LogError(ex, ex.Message);
                return new JsonResult(ex);
            }
        }
    }
}
