using App.Application.DTOs.Common;
using App.Application.DTOs.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface ITaskService
    {
        Task<ApiResponse<TaskDto>> CreateTaskAsync(CreateTaskDto dto, int userId);
        Task<ApiResponse<bool>>MoveTaskAsync(int taskId, MoveTaskDto dto, int userId);
    }
}
