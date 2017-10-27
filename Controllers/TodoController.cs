using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using todoapi.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace todoapi.Controllers {
    [Route("api/[controller]")]
    public class TodoController : Controller {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
            
            if (_context.TodoItems.Count() == 0) {
                _context.TodoItems.Add(new TodoItem { Name = "Item 1"});
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<IEnumerable<TodoItem>> GetAll()
        {
            return await _context.TodoItems.Include(x => x.Tags).ToListAsync();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (item == null) {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item) {
            if (item == null) {
                return BadRequest();
            }

            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Replace(long id, [FromBody] TodoItem item) {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);

            if (todo == null) {
                return NotFound();
            }

            todo.Name = item.Name;
            todo.IsComplete = item.IsComplete;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();

            return new NoContentResult();

        }

        [HttpPatch("{id}")]
        public IActionResult Update(long id, [FromBody] JsonPatchDocument<TodoItem> patch) {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);

            if (todo == null) {
                return NotFound();
            }

            var original = todo.Copy();

            patch.ApplyTo(todo, ModelState);

            if ( ! ModelState.IsValid) {
                return new BadRequestObjectResult(ModelState);
            }

            _context.TodoItems.Update(todo);
            _context.SaveChanges();

            var model = new {
                original = original,
                patched = todo
            };

            return Ok(model);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id) {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);

            if (todo == null) {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();

            return new NoContentResult();
        }
    }
}