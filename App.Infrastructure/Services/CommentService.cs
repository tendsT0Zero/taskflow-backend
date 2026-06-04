using App.Application.DTOs.Comment;
using App.Application.DTOs.Common;
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
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;

        public CommentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<CommentDto>> AddCommentAsync(CreateCommentDto dto, int userId)
        {
            var taskExists = await _context.TaskItems.AnyAsync(t => t.Id == dto.TaskItemId);
            if (!taskExists)
                return new ApiResponse<CommentDto> { Success = false, Message = "Task not found." };

            var comment = new Comment
            {
                Text = dto.Text,
                TaskItemId = dto.TaskItemId,
                UserId = userId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();


            var savedComment = await _context.Comments
                .Include(c => c.User)
                .FirstAsync(c => c.Id == comment.Id);

            var responseDto = new CommentDto
            {
                Id = savedComment.Id,
                Text = savedComment.Text,
                TaskItemId = savedComment.TaskItemId,
                UserId = savedComment.UserId,
                UserName = savedComment.User.FullName,
                CreatedAt = savedComment.CreatedAt
            };

            return new ApiResponse<CommentDto> { Success = true, Message = "Comment added!", Data = responseDto };
        }

        public async Task<ApiResponse<List<CommentDto>>> GetCommentsByTaskAsync(int taskId)
        {
            var comments = await _context.Comments
                .Include(c => c.User)
                .Where(c => c.TaskItemId == taskId)
                .OrderByDescending(c => c.CreatedAt) 
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Text = c.Text,
                    TaskItemId = c.TaskItemId,
                    UserId = c.UserId,
                    UserName = c.User.FullName,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            return new ApiResponse<List<CommentDto>> { Success = true, Data = comments };
        }
    }
}
