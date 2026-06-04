using App.Application.DTOs.Task;
using App.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto model)
        {
            var response = await _taskService.CreateTaskAsync(model, GetUserId());
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPatch("{id}/move")]
        public async Task<IActionResult> MoveTask(int id, [FromBody] MoveTaskDto model)
        {
            var response = await _taskService.MoveTaskAsync(id, model, GetUserId());
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
    }
}
