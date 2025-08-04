namespace todo_api.Models
{
    public class TodoItem
    {
        public int id { get; set; }
        public string? title { get; set; }
        public Boolean completed { get; set; }
    }
}
