using App.Application.DTOs.Common;
using App.Application.DTOs.Workspace;
using App.Application.Interfaces;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Services
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly AppDbContext _context;
        public WorkspaceService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<WorkspaceDto>> CreateWorkspaceAsync(CreateWorkspaceDto dto, int userId)
        {
            try
            {

                var workspace = new Domain.Entities.Workspace
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    CreatedAt = DateTime.UtcNow,
                    OwnerId = userId
                };

                await _context.Workspaces.AddAsync(workspace);
                await _context.SaveChangesAsync();

                var workspaceDto = new WorkspaceDto
                {
                    Id = workspace.Id,
                    Name = workspace.Name,
                    Description = workspace.Description,
                    CreatedAt = workspace.CreatedAt
                };

                return new ApiResponse<WorkspaceDto>
                {
                    Success = true,
                    Message = "Workspace created successfully.",
                    Data = workspaceDto
                };

            }
            catch (Exception ex)
            {
                return new ApiResponse<WorkspaceDto>
                {
                    Success = false,
                    Message = "An error occurred while creating the workspace.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<List<WorkspaceDto>>> GetUserWorkspacesAsync(int userId)
        {
            try
            {
                var workspaces =await _context.Workspaces
                    .Where(w => w.OwnerId == userId)
                    .OrderByDescending(w => w.CreatedAt)
                    .Select(w => new WorkspaceDto
                    {
                        Id = w.Id,
                        Name = w.Name,
                        Description = w.Description,
                        CreatedAt = w.CreatedAt
                    })
                    .ToListAsync();
                return new ApiResponse<List<WorkspaceDto>>
                {
                    Success = true,
                    Message = "Workspaces retrieved successfully.",
                    Data = workspaces
                };

            }
            catch (Exception ex)
            {
                return new ApiResponse<List<WorkspaceDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving workspaces.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
