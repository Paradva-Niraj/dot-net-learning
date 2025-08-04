using Microsoft.AspNetCore.Mvc;
using todo_api.Models;
using todo_api.Services;

namespace todo_api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoServices _service;

        public TodoController(TodoServices service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoItem>> GetAll() => Ok(_service.GetAll());
        [HttpGet("{id}")]
        public ActionResult<TodoItem> Get(int id)
        {
            var item = _service.GetById(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public ActionResult<TodoItem> Create(TodoItem item)
        {
            var created = _service.Create(item);
            return CreatedAtAction(nameof(Get), new { id = created.id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TodoItem item)
        {
            return _service.Update(id, item) ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return _service.Delete(id) ? NoContent() : NotFound();
        }
    }
}
