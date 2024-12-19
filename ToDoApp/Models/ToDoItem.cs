using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ToDoApp.Models
{
    [Table("ToDoItem")]
    public class ToDoItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? ToDoContent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime DueTo { get; set; }
        public Status Status { get; set; } = Status.ToDo;
    }

    public enum Status
    {
        ToDo = 0,
        InProgress = 1,
        Done = 2
    }
}
