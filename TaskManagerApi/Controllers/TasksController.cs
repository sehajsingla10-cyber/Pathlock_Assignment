using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        // In-memory list to simulate a database
        private static List<TaskItem> tasks = new();
        private static int nextId = 1;

        /// <summary>
        /// GET: /api/tasks
        /// Returns all tasks
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetAll()
        {
            return Ok(tasks);
        }

        /// <summary>
        /// GET: /api/tasks/{id}
        /// Returns a single task by ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetById(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound(new { message = "Task not found." });
            return Ok(task);
        }

        /// <summary>
        /// POST: /api/tasks
        /// Adds a new task
        /// </summary>
        [HttpPost]
        public ActionResult<TaskItem> Create([FromBody] TaskItem newTask)
        {
            if (string.IsNullOrWhiteSpace(newTask.Description))
                return BadRequest(new { message = "Description cannot be empty." });

            newTask.Id = nextId++;
            newTask.IsCompleted = false; // default to not completed
            tasks.Add(newTask);

            return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask);
        }

        /// <summary>
        /// PUT: /api/tasks/{id}
        /// Updates a task description
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TaskItem updatedTask)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound(new { message = "Task not found." });

            task.Description = updatedTask.Description;
            return NoContent();
        }

        /// <summary>
        /// PUT: /api/tasks/{id}/toggle
        /// Toggles completion status
        /// </summary>
        [HttpPut("{id}/toggle")]
        public IActionResult Toggle(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound(new { message = "Task not found." });

            task.IsCompleted = !task.IsCompleted;
            return NoContent();
        }

        /// <summary>
        /// DELETE: /api/tasks/{id}
        /// Deletes a task
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound(new { message = "Task not found." });

            tasks.Remove(task);
            return NoContent();
        }
    }
}
