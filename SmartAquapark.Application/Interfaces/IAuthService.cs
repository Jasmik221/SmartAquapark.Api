using System;
using System.Collections.Generic;
using System.Text;

using SmartAquapark.Application.DTOs;

namespace SmartAquapark.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> RegisterAsync(RegisterUserDto dto);

    Task<AuthResponseDto?> LoginAsync(LoginUserDto dto);
}
