using App.Application.DTOs.Project;
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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto model)
        {
            var response = await _projectService.CreateProjectAsync(model, GetUserId());
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectBoard(int id)
        {
            var response = await _projectService.GetProjectBoardAsync(id, GetUserId());
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }
    }
}
