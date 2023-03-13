namespace ToDoCore
{
    public class Note
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
        public Guid ToDoListId { get; set; }
        public ToDoList ToDoList { get; set; } = null!;
        public int Position { get; set; }
    }
}
