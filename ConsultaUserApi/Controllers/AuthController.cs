using Application.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultaUserApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITokenService tokenService;
        private readonly IUserService userService;
        private readonly IPessoaService pessoaService;

        public AuthController(SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            IUserService userService,
            IPessoaService pessoaService)
        {
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.userService = userService;
            this.tokenService = tokenService;
            this.userService = userService;
            this.pessoaService = pessoaService;
        }

        [AllowAnonymous]
        [HttpPost("login")]        
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] LoginUserDto loginUserDto)
        {
            try
            {

                if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

                //var userMan = await this.userManager.FindByEmailAsync(loginUserDto.Email);

                //if (userMan == null)
                //{
                //    return BadRequest("Usuario não encontrado");
                //}

                var result = await this.signInManager.PasswordSignInAsync(loginUserDto.Email, loginUserDto.Password, false, true);

                if (result.Succeeded)
                {
                    UserDto user = new UserDto() { Email = loginUserDto.Email };
                    user.Token = this.tokenService.GenerateToken(loginUserDto.Email);
                    return Ok(new { user });

                }
                return BadRequest("Usuario ou senha invalidos");
            }
            catch (System.Exception ex)
            {
                // TODO
                return BadRequest(ex.Message);
            }           
          
        }


        [HttpPost("register")]
        public async Task<IActionResult> Registrar(RegisterUserDto registerUserDto)
        {
            try
            {

                if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

                // Verificar se a pessoal ja existe
                if (await this.pessoaService.PessoaExists(registerUserDto)) return BadRequest("Usuario ja cadastrado");
                //Gerar Uma pessoa na base
                var pessoaId = await pessoaService.AddPessoa(registerUserDto);
                             
                var result = await this.userService.AddUser(registerUserDto);

                if (!result.Succeeded) return BadRequest(result.Errors);

                #region EMAILCONFIRM
                // var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //    string confirmationLink = Url.Action("ConfirmEmail", 
                //   "Auth", new { userid = user.Id, 
                //    token = confirmationToken }, 
                //    Request.Scheme);

                //     await emailSender.SendConfirmationEmailAsync(registerUserDto.Email, "Confirme seu cadastro", "",confirmationLink);
                #endregion
                              

                UserDto userDto = new UserDto();
                userDto.Token = this.tokenService.GenerateToken(registerUserDto.Email);
                userDto.Email = registerUserDto.Email;
                return Ok(new { userDto });

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
