using Application.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signInManager;

        public UserService(UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<User> signInManager
        )
        {            
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

       
        public async Task<IdentityResult> AddUser(RegisterUserDto registerUserDto)
        {
            var user = new User
            {
                //PessoaId = pessoaId,
                UserName = registerUserDto.Email,
                Email = registerUserDto.Email,
                EmailConfirmed = false
            };

            if (string.IsNullOrEmpty(registerUserDto.Role))
                await this.userManager.AddToRoleAsync(user, "User");
            else
            {
                bool roleExist = await this.roleManager.RoleExistsAsync(registerUserDto.Role);
                if (roleExist)
                    await this.userManager.AddToRoleAsync(user, registerUserDto.Role);
                else throw new Exception("Role não existe");
            }

            await this.signInManager.SignInAsync(user, false);

            var result = await this.userManager.CreateAsync(user, registerUserDto.Password);
            return result;
        }

        public async Task<List<IdentityRole>> ObterPermissoes(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);

            var userRole = await this.userManager.GetRolesAsync(user);

            var roles = this.roleManager.Roles.ToList();

            if (userRole.FirstOrDefault().ToUpper() != "ADMIN")
            {
                roles.Remove(roles.FirstOrDefault(x => x.Name.Contains("Admin")));
            }

            return roles;
        }

       
    }
}
