using Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<List<IdentityRole>> ObterPermissoes(string email);

        Task<IdentityResult> AddUser(RegisterUserDto registerUserDto);

        

    }
}
