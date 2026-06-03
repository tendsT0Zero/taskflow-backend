using App.Application.DTOs.Common;
using App.Application.DTOs.Workspace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IWorkspaceService
    {
        Task<ApiResponse<WorkspaceDto>> CreateWorkspaceAsync(CreateWorkspaceDto dto, int userId);
        Task<ApiResponse<List<WorkspaceDto>>> GetUserWorkspacesAsync(int userId);
    }
}
