using App.Application.DTOs.Comment;
using App.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto model)
        {
            var response = await _commentService.AddCommentAsync(model, GetUserId());
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTaskComments(int taskId)
        {
            var response = await _commentService.GetCommentsByTaskAsync(taskId);
            return Ok(response);
        }
    }
}
