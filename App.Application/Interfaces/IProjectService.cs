using App.Application.DTOs.Common;
using App.Application.DTOs.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IProjectService
    {
        Task<ApiResponse<ProjectDto>> CreateProjectAsync(CreateProjectDto dto, int userId);
        Task<ApiResponse<ProjectDto>> GetProjectBoardAsync(int projectId, int userId);
    }
}
