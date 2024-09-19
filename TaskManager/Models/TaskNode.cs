﻿namespace TaskManagerApi.Models
{
    /// <summary>
    /// Model of task
    /// </summary>
    public class TaskNode
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}
