using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace todoapi.Models
{
   public class TodoContext : DbContext {
       public TodoContext(DbContextOptions<TodoContext> options) : base(options) {}

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public override int SaveChanges() {
            var objectStateEntries = ChangeTracker.Entries()
                .Where (t => t.State == EntityState.Modified || t.State == EntityState.Added).ToList();

            var currentTime = DateTime.UtcNow;

            foreach (var entry in objectStateEntries)
            {
                var entityBase = entry.Entity;
                if (entityBase == null) continue;
                if (entry.State == EntityState.Added)
                {
                    ((dynamic) entityBase).CreatedAt = currentTime;
                }
                ((dynamic) entityBase).LastModifiedAt = currentTime;
            }
           return base.SaveChanges();
       }
   } 
}