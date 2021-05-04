using Application.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Infra.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository pessoaRepository;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public PessoaService(IPessoaRepository pessoaRepository,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            this.pessoaRepository = pessoaRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<int> AddPessoa(RegisterUserDto registerUserDto)
        {
            var pessoa = await pessoaRepository.GetPessoaByCpf(registerUserDto.Cpf);

            if (pessoa != null) throw new Exception("Cpf já cadastrado");

            var newpessoa = new PessoaDto
            {
                Nome = registerUserDto.Nome,
                Sobrenome = registerUserDto.Sobrenome,
                CPF = registerUserDto.Cpf,
                DataNascimento = registerUserDto.DataNascimento,
                Telefone = registerUserDto.Celular
            };

            var result = await this.pessoaRepository.CreatePessoa(newpessoa);

            return result;
        }

        public async Task<bool> PessoaExists(RegisterUserDto registerUser)
        {
            var user = this.userManager.FindByEmailAsync(registerUser.Email);

            var pessoa = this.pessoaRepository.GetPessoaByCpf(registerUser.Cpf);


            if (pessoa.Result != null || user.Result != null)
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }
    }
}
