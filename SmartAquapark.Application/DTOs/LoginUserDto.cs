using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAquapark.Application.DTOs
{
    public class LoginUserDto
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
