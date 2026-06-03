using App.Application.DTOs;
using App.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> RegisterAsync(RegisterDto model);
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto model);
    }
}
