using App.Application.DTOs.Comment;
using App.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface ICommentService
    {
        Task<ApiResponse<CommentDto>> AddCommentAsync(CreateCommentDto dto, int userId);
        Task<ApiResponse<List<CommentDto>>> GetCommentsByTaskAsync(int taskId);
    }
}
