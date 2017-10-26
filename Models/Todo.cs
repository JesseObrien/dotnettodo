using System;

namespace todoapi.Models {
    public class TodoItem {

        public TodoItem() {
            var now = DateTime.Now;
            LastModifiedAt = now;
            CreatedAt = now;
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public TodoItem Copy() {
            return new TodoItem { Id = this.Id, Name = this.Name, IsComplete = this.IsComplete };
        }
    }
}