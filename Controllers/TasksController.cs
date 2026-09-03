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
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll()
        {
            var tasks = await _appDbContext.TasksManager
            .Select(t => new TaskDto()
            {
                Id = t.Id,
                TaskName = t.TaskName,
                TaskStatus = t.TaskStatus,
                DeadLine = t.DeadLine,
                Description = t.Description,
                Done = t.Done
            }).ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {   
            return Ok(await FilterByStatus(status));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var taskById = await _appDbContext.TasksManager.FindAsync(id);
            if (await TaskExists(id) == false)
            {
               return BadRequest();
            }

            try
            {
                taskById.TaskStatus = status;
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw new Exception("Task don't exist!");
                }
            }
            return Ok(taskById);
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

        private async Task<List<TaskDto>> FilterByStatus(string status)
        {
            var tasks = await _appDbContext.TasksManager
            .Select(t => new TaskDto()
            {
                Id = t.Id,
                TaskName = t.TaskName,
                TaskStatus = t.TaskStatus,
                DeadLine = t.DeadLine,
                Description = t.Description,
                Done = t.Done
            }).ToListAsync();
            return tasks.Where(t => t.TaskStatus == status).ToList();
        }
    }
}