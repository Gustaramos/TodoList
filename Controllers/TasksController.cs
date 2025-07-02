using CRUD.Data;
using CRUD.DTO;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public TasksController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(TaskItem task)
        {
            _appDbContext.TasksManager.Add(task);
            await _appDbContext.SaveChangesAsync();

            return Ok(task);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAsync()
        {
            var tasks = await _appDbContext.TasksManager
            .Select(t => new TaskDto()
            {
                Id = t.Id,
                TaskName = t.TaskName,
                TaskStatus = t.TaskStatus,
                DeadLine = t.DeadLine,
                Description = t.Description
            }).ToListAsync();

            return Ok(tasks);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(int id, TaskItem task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            var taskById = await _appDbContext.TasksManager.FindAsync(id);
            taskById.TaskStatus = task.TaskStatus;

            try
            {
                await _appDbContext.TasksManager.FindAsync(id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await TaskExists(id))
                {
                    await _appDbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Task don't exist!");
                }
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var task = await _appDbContext.TasksManager.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            _appDbContext.TasksManager.Remove(task);
            await _appDbContext.SaveChangesAsync();

            return Ok(task);
        }

        private async Task<bool> TaskExists(int id)
        {
            return await _appDbContext.TasksManager.AnyAsync(t => t.Id == id);
        }

        private string FilterByStatus(string status)
        {

            var retrieveTask = _appDbContext.TasksManager.Find(status);
            var getTaskByStatus = retrieveTask.TaskStatus;
            return getTaskByStatus;
        }

        

    }


}