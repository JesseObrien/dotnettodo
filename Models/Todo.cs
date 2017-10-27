using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace todoapi.Models {
    public class TodoItem {

        public TodoItem() {
            var now = DateTime.Now;
            LastModifiedAt = now;
            CreatedAt = now;
        }

        [Key, Required]
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual List<Tag> Tags { get; set; }

        public TodoItem Copy() {
            return new TodoItem { Id = this.Id, Name = this.Name, IsComplete = this.IsComplete };
        }
    }
}