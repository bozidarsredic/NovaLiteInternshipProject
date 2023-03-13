namespace ToDoCore
{
    public class ToDoList
    {
        public string Owner { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public IList<Note> Notes { get; set; } = new List<Note>();
        public DateTime? Remainder { get; set; }
        public bool IsRemind { get; set; }
        public ToDoList() { }
        public int Position { get; set; }
        public DateTime? ShareTime { get; set; }

    }
}
