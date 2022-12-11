namespace TodoApp.Domain.Todo
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public bool State { get; set; } = false;
    }
}