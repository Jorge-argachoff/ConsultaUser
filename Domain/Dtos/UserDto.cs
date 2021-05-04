using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
