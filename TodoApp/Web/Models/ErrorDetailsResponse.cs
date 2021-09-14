using System.Text.Json.Serialization;

namespace TodoApp.Web.Models
{
    public class ErrorDetailsResponse
    {
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}