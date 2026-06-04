using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs.Comment
{
    public class CreateCommentDto
    {
        [Required]
        public int TaskItemId { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
