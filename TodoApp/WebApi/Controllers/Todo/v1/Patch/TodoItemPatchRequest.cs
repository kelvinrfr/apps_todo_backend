using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoApp.WebApi.Controllers.Todo.v1.Patch
{
    public class TodoItemPatchRequest
    {
        [MaxLength(100)]
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("state")]
        public bool State { get; set; }
    }
}