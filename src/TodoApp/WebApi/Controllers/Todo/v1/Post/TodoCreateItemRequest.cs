using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoApp.WebApi.Controllers.Todo.v1.Post
{
    public class TodoCreateItemRequest
    {
        [MaxLength(100)]
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}