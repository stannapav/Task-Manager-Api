using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Interface;
using TaskManagerApi.Models;
using TaskManagerApi.Repository;

namespace TaskManagerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            //Add SignalR that works with websockets
            builder.Services.AddSignalR();

            //Idependency Injection
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddSingleton<ITaskHub, TaskHub>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Adding inMemory database
            builder.Services.AddDbContext<DataContext>
                (opt => opt.UseInMemoryDatabase("TaskManagerDb"));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //Sets channel route for websocket connection
            app.UseEndpoints(endpoints => 
            { 
                endpoints.MapHub<TaskHub>("/taskHub"); 
            });

            app.MapControllers();

            app.Run();
        }
    }
}
