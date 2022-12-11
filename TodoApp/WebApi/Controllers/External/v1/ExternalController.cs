using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApp.WebApi.Configuration;

namespace TodoApp.WebApi.Controllers.External.v1
{
    [ApiController]
    [Route("v1/external")]
    [Produces("application/json")]
    public class ExternalController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ExternalConfiguration _configuration;

        // TODO: expose HTTP Client through the infrastructure project
        public ExternalController(HttpClient httpClient, TodoAppConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration.External;
        }
        /// <summary>
        /// External endpoint mainly for testing external calls within this service.
        /// </summary>
        /// <returns>Ok</returns>
        /// <response code="200">Returns the content of the external GET request</response>
        /// <response code="500">Invalid server configuration or Unexpected error</response>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            if (string.IsNullOrWhiteSpace(_configuration?.Url))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "invalid external url");
            }

            var result = await _httpClient.GetAsync(_configuration.Url);

            return Ok(await result.Content.ReadAsStringAsync());
        }
    }
}