using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TaskManagerApi.Interface;

namespace TaskManagerApi.Models
{
    /// <summary>
    /// TaksHub is an instance that can send and recive websocket messages
    /// </summary>
    public class TaskHub : Hub, ITaskHub
    {
        private readonly IHubContext<TaskHub> _hubContext;

        public TaskHub(IHubContext<TaskHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Sends client a string message in JSON form of Task
        /// </summary>
        /// 
        /// <param name="task">task that will be send to client</param>
        /// 
        /// <returns>
        /// Returns an operation to do to send a message of Json of task
        /// </returns>
        public async Task SendMessage(TaskNode task)
        {
            string jsonTask = JsonConvert.SerializeObject(task);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", jsonTask);
        }
    }
}
