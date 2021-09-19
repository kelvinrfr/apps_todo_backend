using System.Text.Json.Serialization;

namespace TodoApp.Web.Controllers.v1
{
    public class TodoItemResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("state")]
        public bool State { get; set; }
    }
}