using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Controllers;
using TaskManagerApi.Interface;
using TaskManagerApi.Models;

namespace TaskManagerApi.Tests.Controller
{
    public class TaskControllerTests
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskHub _hubContext;

        public TaskControllerTests()
        {
            _taskRepository = A.Fake<ITaskRepository>();
            _hubContext = A.Fake<ITaskHub>();
        }

        [Fact]
        public void TaskController_GetTasks_ReturnOk()
        {
            var tasks = A.Fake<ICollection<TaskNode>>();

            var controller = new TaskController(_taskRepository, _hubContext);

            var result = controller.GetTasks();

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void TaskController_CreateTask_ReturnOk()
        {
            var taskCreate = A.Fake<TaskNode>();

            A.CallTo(() => _taskRepository.FindTaskSameTitle(taskCreate)).Returns(null);
            A.CallTo(() => _taskRepository.CreateTask(taskCreate)).Returns(true);
            A.CallTo(() => _hubContext.SendMessage(taskCreate));

            var controller = new TaskController(_taskRepository, _hubContext);

            var result = controller.CreateTask(taskCreate);

            Assert.NotEqual(taskCreate.Title, string.Empty);
            Assert.NotEqual(taskCreate.Description, string.Empty);
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void TaskController_UpdateTask_ReturnNoContent()
        {
            int id = 0;
            var updateTask = A.Fake<TaskNode>();

            A.CallTo(() => _taskRepository.TaskExist(id)).Returns(true);
            A.CallTo(() => _taskRepository.UpdateTask(updateTask)).Returns(true);
            A.CallTo(() => _hubContext.SendMessage(updateTask));

            var controller = new TaskController(_taskRepository, _hubContext);

            var result = controller.UpdateTask(id, updateTask);

            Assert.NotEqual(updateTask.Title, string.Empty);
            Assert.NotEqual(updateTask.Description, string.Empty);
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Fact]
        public void TaskController_DeleteTask_ReturnNoContent()
        {
            int id = 1;
            var taskToDelete = A.Fake<TaskNode>();

            A.CallTo(() => _taskRepository.TaskExist(id)).Returns(true);
            A.CallTo(() => _taskRepository.GetTask(id)).Returns(taskToDelete);
            A.CallTo(() => _taskRepository.DeleteTask(taskToDelete)).Returns(true);

            var controller = new TaskController(_taskRepository, _hubContext);

            var result = controller.DeleteTask(id);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
