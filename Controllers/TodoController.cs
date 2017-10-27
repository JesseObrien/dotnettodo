using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using todoapi.Models;
using System.Linq;
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
            return await _context.TodoItems.ToListAsync();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
            if (item == null) {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoItem item) {
            if (item == null) {
                return BadRequest();
            }

            await _context.TodoItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Replace(long id, [FromBody] TodoItem item) {
            var todo = await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null) {
                return NotFound();
            }

            todo.Name = item.Name;
            todo.IsComplete = item.IsComplete;

            _context.TodoItems.Update(todo);
            await _context.SaveChangesAsync();

            return new NoContentResult();

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] JsonPatchDocument<TodoItem> patch) {
            var todo = await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null) {
                return NotFound();
            }

            var original = todo.Copy();

            patch.ApplyTo(todo, ModelState);

            if ( ! ModelState.IsValid) {
                return new BadRequestObjectResult(ModelState);
            }

            _context.TodoItems.Update(todo);
            await _context.SaveChangesAsync();

            var model = new {
                original = original,
                patched = todo
            };

            return Ok(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id) {
            var todo = await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null) {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}