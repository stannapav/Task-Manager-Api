using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Interface;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers
{
    /// <summary>
    /// Controller that has CRUD operations for model TaskNode.
    /// 
    /// Has:
    /// POST CreateTask that creates task
    /// PUT UpdateTasl that changes already existing task
    /// GET GetTasks that gets all tasks
    /// DELETE DeleteTask that deletes task by id
    /// </summary>
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskHub _hubContext;

        public TaskController(ITaskRepository taskRepository, ITaskHub hubContext)
        {
            _taskRepository = taskRepository;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Creates task object and save it to inMemory and send json of that task to all clients
        /// </summary>
        /// 
        /// <param name="taskCreate">Takes parametr from request body for task</param>
        /// 
        /// <returns>
        /// Returns BadRequest(400) if request body wasnt a task or title/describtion of task was empty
        /// Returns Ok(200) if successfully created
        /// Returns Unprocessable Content(422) if task like this already exist
        /// Returns Internal Server Error(500) if the saving task to memory was failed
        /// </returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTask([FromBody] TaskNode taskCreate)
        {
            if (taskCreate == null)
                return BadRequest(ModelState);

            if (taskCreate.Title == string.Empty || taskCreate.Description == string.Empty)
                return BadRequest(ModelState);

            var task = _taskRepository.FindTaskSameTitle(taskCreate);

            if(task != null)
            {
                ModelState.AddModelError("", "Task already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_taskRepository.CreateTask(taskCreate))
            {
                ModelState.AddModelError("", "Someting went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _hubContext.SendMessage(taskCreate);

            return Ok("Successfully created");
        }

        /// <summary>
        /// Gets all tasks in a form of a list of objects
        /// </summary>
        /// 
        /// <returns>
        /// Returns Ok(200) if got all tasks from memory
        /// Returns BadRequest(400) if data is incorrect
        /// </returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TaskNode>))]
        public IActionResult GetTasks()
        {
            var tasks = _taskRepository.GetTasks();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tasks);
        }

        /// <summary>
        /// Updates or change the info of task of some id and send json of that task to all clients
        /// </summary>
        /// 
        /// <param name="id">Id of a task to change</param>
        /// <param name="updateTask">Changed task</param>
        /// 
        /// <returns>
        /// Returns BadRequest(400) if request body wasnt a task or title/describtion of task was empty,
        ///     or if wrong id of task that need to be changed, or data isnt right
        /// Returns Internal Server Error(500) if the saving task to memory was failed
        /// Returns NotFound(404) if there's no task with that id
        /// Returns NoContent(204) if saving was succesful
        /// </returns>
        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTask(int id, [FromBody] TaskNode updateTask)
        {
            if (updateTask == null)
                return BadRequest(ModelState);

            if (updateTask.Title == string.Empty || updateTask.Description == string.Empty)
                return BadRequest(ModelState);

            if (id != updateTask.Id)
                return BadRequest(ModelState);

            if (!_taskRepository.TaskExist(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_taskRepository.UpdateTask(updateTask))
            {
                ModelState.AddModelError("", "Something went wrong updating task");
                return StatusCode(500, ModelState);
            }

            _hubContext.SendMessage(updateTask);

            return NoContent();
        }

        /// <summary>
        /// Deletes task of that id
        /// </summary>
        /// 
        /// <param name="id">Id of task to delete</param>
        /// 
        /// <returns>
        /// Returns NotFound(404) if there's no task with that id
        /// Returns BadRequest(400) if data is incorrect
        /// Returns Internal Server Error(500) if the deleting from memory was failed
        /// Returns NoContent(204) if deleting was succesful
        /// </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTask(int id)
        {
            if (!_taskRepository.TaskExist(id))
                return NotFound();

            var taskToDelete = _taskRepository.GetTask(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_taskRepository.DeleteTask(taskToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting task");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
