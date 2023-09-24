using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserTaskApi.Controllers
{
    [Authorize] // Requires authorization for all actions in this controller
    public class TaskController : BaseApiController
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public IActionResult GetTasks()
        {
            var tasks = _taskRepository.GetAll(); // Retrieve a list of tasks from the repository

            // Return a 200 OK response with the list of tasks
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public IActionResult GetTask(int id)
        {
            // Retrieve a single task by its ID from the repository
            var task = _taskRepository.GetById(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost]
        public IActionResult CreateTask(TaskDto taskDto)
        {
            // Create a new task entity from the provided TaskDto
            var task = new Task
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                AssigneeID = taskDto.AssigneeID,
                DueDate = taskDto.DueDate
            };

            _taskRepository.Add(task);

            return Ok(task);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, TaskDto taskDto)
        {
            // Retrieve an existing task by its ID from the repository
            var existingTask = _taskRepository.GetById(id);

            if (existingTask == null)
            {
                return NotFound();
            }

            // Update the task's properties
            existingTask.Title = taskDto.Title;
            existingTask.Description = taskDto.Description;
            existingTask.AssigneeID = taskDto.AssigneeID;
            existingTask.DueDate = taskDto.DueDate;

            _taskRepository.Update(existingTask);

            return Ok(existingTask);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            // Retrieve an existing task by its ID from the repository
            var existingTask = _taskRepository.GetById(id);

            if (existingTask == null)
            {
                return NotFound();
            }

            // Remove the task from the repository 
            _taskRepository.Remove(id);

            return Ok(new { Message = "Task deleted successfully" }); // Return a 200 OK response with a success message
        }
    }
}