using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoApp.Api.Http.Controllers.Todo.v1.Put
{
	public class TodoPutItemRequest
	{
		[MaxLength(100)]
		[JsonPropertyName("description")]
		public string Description { get; set; }
		[JsonPropertyName("state")]
		public bool State { get; set; }
	}
}