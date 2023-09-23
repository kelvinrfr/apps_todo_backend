using System.Text.Json.Serialization;

namespace TodoApp.WebApi.Common
{
    public class ErrorDetailsResponse
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}