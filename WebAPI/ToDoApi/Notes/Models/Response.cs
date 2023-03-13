using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Notes.Models
{
    public class Response
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public IList<GetNotesModel> Notes { get; set; } = new List<GetNotesModel>();
        public DateTime? Remainder { get; set; }
        public bool IsRemind { get; set; }
        [Required]
        public int Position { get; set; }
        public string Owner { get; set; } = string.Empty;
        public DateTime? ShareTime { get; set; }
    }
}
