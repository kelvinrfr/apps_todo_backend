using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoApp.Web.Controllers.v1
{
    public class TodoItemCreateRequest
    {
        [MaxLength(100)]
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}