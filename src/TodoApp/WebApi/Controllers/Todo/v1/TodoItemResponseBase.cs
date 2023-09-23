using System.Text.Json.Serialization;

namespace TodoApp.Api.Http.WebApi.Controllers.Todo.v1
{
    public abstract class TodoItemResponseBase
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("state")]
        public bool State { get; set; }
    }
}