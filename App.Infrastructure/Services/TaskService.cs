using App.Application.DTOs.Common;
using App.Application.DTOs.Task;
using App.Application.Interfaces;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Services
{
    public class TaskService : ITaskService
    {
        public readonly AppDbContext _context;
        public TaskService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<TaskDto>> CreateTaskAsync(CreateTaskDto dto, int userId)
        {
            try
            {
                var column = await _context.BoardColumns.FindAsync(dto.ColumnId);
                if (column == null)
                    return new ApiResponse<TaskDto> { Success = false, Message = "Column not found." };

                var maxOrder = await _context.TaskItems
                    .Where(t => t.ColumnId == dto.ColumnId)
                    .MaxAsync(t => (int?)t.Order) ?? -1;

                var priorityEnum = Enum.TryParse<TaskPriority>(dto.Priority, true, out var parsed)
                    ? parsed : TaskPriority.Medium;

                var task = new TaskItem
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Priority = priorityEnum,
                    ColumnId = dto.ColumnId,
                    AssigneeId = dto.AssigneeId,
                    Order = maxOrder + 1 
                };

                _context.TaskItems.Add(task);
                await _context.SaveChangesAsync();

                var taskDto = new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Priority = task.Priority.ToString(),
                    Order = task.Order,
                    ColumnId = task.ColumnId,
                    AssigneeId = task.AssigneeId,
                    CreatedAt = task.CreatedAt
                };

                return new ApiResponse<TaskDto> { Success = true, Message = "Task created!", Data = taskDto };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TaskDto>
                {
                    Success = false,
                    Message = "Failed to create task",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> MoveTaskAsync(int taskId, MoveTaskDto dto, int userId)
        {
            try
            {
                var task = await _context.TaskItems.FindAsync(taskId);
                if (task == null)
                    return new ApiResponse<bool> { Success = false, Message = "Task not found." };

                task.ColumnId = dto.NewColumnId;
                task.Order = dto.NewOrder;

                _context.TaskItems.Update(task);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool> { Success = true, Message = "Task moved successfully!", Data = true };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Failed to create task",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
