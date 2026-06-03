using App.Application.DTOs.Common;
using App.Application.DTOs.Project;
using App.Application.Interfaces;
using App.Domain.Entities;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<ProjectDto>> CreateProjectAsync(CreateProjectDto dto, int userId)
        {
            try
            {
                // check if workspace exists and belongs to the user
                var workspace = await _context.Workspaces
                    .FirstOrDefaultAsync(w => w.Id == dto.WorkspaceId && w.OwnerId == userId);

                if (workspace == null)
                    return new ApiResponse<ProjectDto> { Success = false, Message = "Workspace not found or access denied." };

                // create project
                var project = new Project
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    WorkspaceId = dto.WorkspaceId
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync(); // Save to get the project ID

                // create default columns
                var defaultColumns = new List<BoardColumn>
                    {
                        new BoardColumn { Name = "To Do", Order = 0, ProjectId = project.Id },
                        new BoardColumn { Name = "In Progress", Order = 1, ProjectId = project.Id },
                        new BoardColumn { Name = "Done", Order = 2, ProjectId = project.Id }
                    };

                _context.BoardColumns.AddRange(defaultColumns);
                await _context.SaveChangesAsync();

                // return project with columns
                return new ApiResponse<ProjectDto>
                {
                    Success = true,
                    Message = "Project created successfully with default columns!",
                    Data = new ProjectDto
                    {
                        Id = project.Id,
                        Name = project.Name,
                        Description = project.Description,
                        Columns = defaultColumns.Select(c => new BoardColumnDto { Id = c.Id, Name = c.Name, Order = c.Order }).ToList()
                    }
                };
            }
            catch(Exception ex)
            {
                return new ApiResponse<ProjectDto>
                {
                    Success = false,
                    Message = "Failed to create project.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<ProjectDto>> GetProjectBoardAsync(int projectId, int userId)
        {
            try
            {
                // fetch project with columns, ensuring the user has access
                var project = await _context.Projects
                    .Include(p => p.Columns.OrderBy(c => c.Order)) // Include columns ordered by their 'Order' property
                    .FirstOrDefaultAsync(p => p.Id == projectId && p.Workspace.OwnerId == userId);

                if (project == null)
                    return new ApiResponse<ProjectDto> { Success = false, Message = "Project not found." };

                var projectDto = new ProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    Columns = project.Columns.Select(c => new BoardColumnDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Order = c.Order
                    }).ToList()
                };

                return new ApiResponse<ProjectDto> { Success = true, Data = projectDto };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProjectDto>
                {
                    Success = false,
                    Message = "Failed to create project.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
