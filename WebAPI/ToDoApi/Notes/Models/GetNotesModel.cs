using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Notes.Models
{
    public class GetNotesModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        [Required]
        public bool IsComplete { get; set; }
        public Guid ToDoListId { get; set; }
        [Required]
        public int Position { get; set; }
    }
}
