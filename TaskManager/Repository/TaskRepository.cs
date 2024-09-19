using TaskManagerApi.Data;
using TaskManagerApi.Interface;
using TaskManagerApi.Models;

namespace TaskManagerApi.Repository
{
    /// <summary>
    /// Business logic how data is getting send, created or deleted in memory
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly DataContext _context;

        public TaskRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds task data in memory
        /// </summary>
        /// 
        /// <param name="task">Task to add to memory</param>
        /// 
        /// <returns>
        /// Returns true if saving data to memory is succsesful
        /// </returns>
        public bool CreateTask(TaskNode task)
        {
            _context.Add(task);
            return Save();
        }

        /// <summary>
        /// Deletes task data from memory
        /// </summary>
        /// 
        /// <param name="task">Task to delete from memory</param>
        /// 
        /// <returns>
        /// Returns true if deleting data from memory is succsesful
        /// </returns>
        public bool DeleteTask(TaskNode task)
        {
            _context.Remove(task);
            return Save();
        }

        /// <summary>
        /// Gets task by id
        /// </summary>
        /// 
        /// <param name="id">id of task to get</param>
        /// 
        /// <returns>
        /// Returns task by id or returns default data
        /// </returns>
        public TaskNode GetTask(int id)
        {
            return _context.Tasks.Where(t => t.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Gets all tasks
        /// </summary>
        /// 
        /// <returns>
        /// Returns all tasks in a form of list
        /// </returns>
        public ICollection<TaskNode> GetTasks()
        {
            return _context.Tasks.ToList();
        }

        /// <summary>
        /// Find if there is already a task with a same name and returns it
        /// </summary>
        /// 
        /// <param name="task">Task to check on unique title</param>
        /// 
        /// <returns>Will return already existing task if they have similar title or returns null</returns>
        public TaskNode? FindTaskSameTitle(TaskNode task)
        {
            return GetTasks().Where(t => t.Title.Trim().ToUpper() == task.Title.TrimEnd().ToUpper())
                .FirstOrDefault();
        }

        /// <summary>
        /// Saves data to memory
        /// </summary>
        /// 
        /// <returns>
        /// Returns true if saving to memory is succsesful
        /// </returns>
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        /// <summary>
        /// Check if task by that id exist
        /// </summary>
        /// 
        /// <param name="id">Id of task to check</param>
        /// 
        /// <returns>
        /// Returns true if task by that id exist
        /// </returns>
        public bool TaskExist(int id)
        {
            return _context.Tasks.Any(t => t.Id == id);
        }

        /// <summary>
        /// Updates data of task
        /// </summary>
        /// 
        /// <param name="task">Changed task to update</param>
        /// 
        /// <returns>
        /// Returns true if saving to memory is succsesful
        /// </returns>
        public bool UpdateTask(TaskNode task)
        {
            _context.Update(task);
            return Save();
        }
    }
}
