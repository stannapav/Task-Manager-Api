using TaskManagerApi.Models;

namespace TaskManagerApi.Interface
{
    /// <summary>
    /// Interface of taskRepository. An abstract set of rules for future taskRepository
    /// Separates business logic from what user sees
    /// </summary>
    public interface ITaskRepository
    {
        bool CreateTask(TaskNode task);
        ICollection<TaskNode> GetTasks();
        TaskNode GetTask(int id);
        bool UpdateTask(TaskNode task);
        bool DeleteTask(TaskNode task);
        bool TaskExist(int id);
        bool Save();
        TaskNode? FindTaskSameTitle(TaskNode task);
    }
}
