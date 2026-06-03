using App.Application.DTOs.Workspace;
using App.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceService _workspaceService;

        public WorkspaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        private int GetUserId() => int.Parse(User.Claims.First(c => c.Type == "id").Value);


        [HttpGet()]
        public async Task<IActionResult> MyWorkspaces()
        {
            var userId = GetUserId();
            var response = await _workspaceService.GetUserWorkspacesAsync(userId);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateWorkspace(CreateWorkspaceDto dto)
        {
            var userId = GetUserId();
            var response = await _workspaceService.CreateWorkspaceAsync(dto, userId);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);

        }
    }
}
