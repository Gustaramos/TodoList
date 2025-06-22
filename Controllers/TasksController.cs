using System.Runtime.InteropServices;
using CRUD.Data;
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
        public async Task<IActionResult> AddTask(Tasks task)
        {
            _appDbContext.TasksManager.Add(task);
            await _appDbContext.SaveChangesAsync();

            return Ok(task);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetTaskBy()
        {
            var tasksResult = await _appDbContext.TasksManager
            .Select(task => TasksToDTO(task))
            .ToListAsync();

            // var temp = await _appDbContext.TasksManager.
            // FindAsync(FilterByStatus(taskStatus)
            // .ToList());

            await _appDbContext.SaveChangesAsync();
            return Ok(tasksResult);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditTask(int id, Tasks task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            var taskById = await _appDbContext.TasksManager.FindAsync(id);
            taskById.TaskStatus = task.TaskStatus;

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcludeTask(int id)
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

        private bool TaskExists(int id)
        {
            _appDbContext.TasksManager.Find(id);
            return true;
        }

        private string FilterByStatus(string status)
        {

            var retrieveTask = _appDbContext.TasksManager.Find(status);
            var getTaskByStatus = retrieveTask.TaskStatus;
            return getTaskByStatus;
        }

        private static Tasks TasksToDTO(Tasks task)
        {
            Tasks tasksDTO = new()
            {
                Id = task.Id,
                TaskName = task.TaskName,
                TaskStatus = task.TaskStatus,
                DeadLine = task.DeadLine,
                Description = task.Description
            };
            return tasksDTO; 
        }

    }
}