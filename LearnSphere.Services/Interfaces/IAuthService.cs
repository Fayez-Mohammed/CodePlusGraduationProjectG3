using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<RegisterResponseDTO>> RegisterAsync(RegisterDTO registerDTO);
        Task<Result<LoginResponseDTO>> LoginAsync(LoginDTO loginDTO);
    }
}
