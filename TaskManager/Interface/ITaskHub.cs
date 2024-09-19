using TaskManagerApi.Models;

namespace TaskManagerApi.Interface
{
    /// <summary>
    /// Interface of TaskHub and sets rule of it behavior
    /// </summary>
    public interface ITaskHub
    {
        public Task SendMessage(TaskNode task);
    }
}
