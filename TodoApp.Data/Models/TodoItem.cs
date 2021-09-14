using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Data.Models
{
    [Table("todoitem")]
    public class TodoItem
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public bool State { get; set; } = false;
    }
}