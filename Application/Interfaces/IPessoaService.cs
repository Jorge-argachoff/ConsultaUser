using Domain.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPessoaService
    {
        Task<int> AddPessoa(RegisterUserDto registerUser);

        Task<bool> PessoaExists(RegisterUserDto registerUser);
    }
}
