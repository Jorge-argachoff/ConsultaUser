using Dapper;
using Domain.Dtos;
using Domain.Entities;
using Infra.Dapper;
using Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositories
{

    public class PessoaRepository:IPessoaRepository
    {


        public DapperDb Db { get; set; }
        public PessoaRepository(DapperDb db = null)
        {
            Db = db;
        }
        public async Task<IEnumerable<PessoaDto>> GetAll()
        {
            var query = @"SELECT Id
                              ,Nome
                              ,Sobrenome
                              ,Endereco
                              ,Numero
                              ,Bairro
                              ,Complemento
                              ,CEP
                              ,CPF
                              ,RG
                              ,Telefone
                              ,DataNascimento
                          FROM Tb_Pessoa";

            var result = await Db.SqlConnection.QueryAsync<PessoaDto>(query);

            return result;
        }

        public async Task<PessoaDto> GetPessoaByCpf(string cpf)
        {
            var query = @"SELECT Id
                              ,Nome
                              ,Sobrenome
                              ,Endereco
                              ,Numero
                              ,Bairro
                              ,Complemento
                              ,CEP
                              ,CPF
                              ,RG
                              ,Telefone
                              ,DataNascimento
                          FROM Tb_Pessoa
                          WHERE CPF = @cpf";

            var result = await Db.SqlConnection.QueryFirstOrDefaultAsync<PessoaDto>(query, new { cpf });

            return result;
        }

        public async Task<int> CreatePessoa(PessoaDto pessoa)
        {

            var args = new
            {
                DataCriacao = DateTime.UtcNow,
                Nome = pessoa.Nome
                ,
                Sobrenome = pessoa.Sobrenome
                ,
                Endereco = pessoa.Endereco
                ,
                Numero = pessoa.Numero
                ,
                Bairro = pessoa.Bairro
                ,
                CPF = pessoa.CPF
                ,
                CEP = pessoa.CEP
                ,
                RG = pessoa.RG
                ,
                Telefone = pessoa.Telefone
                ,
                DataNascimento = pessoa.DataNascimento
                ,
                Complemento = pessoa.Complemento
            };

            var query = @"INSERT INTO Tb_Pessoa
                            ([DataCriacao]
                            ,[Nome]
                            ,[Sobrenome]
                            ,[Endereco]
                            ,[Numero]
                            ,[Bairro]
                            ,[CPF]
                            ,[CEP]
                            ,[RG]
                            ,[Telefone]
                            ,[DataNascimento]
                            ,[Complemento])
                        VALUES
                            (@DataCriacao
                            ,@Nome
                            ,@Sobrenome
                            ,@Endereco
                            ,@Numero
                            ,@Bairro
                            ,@CPF
                            ,@CEP
                            ,@RG
                            ,@Telefone
                            ,@DataNascimento
                            ,@Complemento);
                            SELECT SCOPE_IDENTITY()";

            var id = await Db.SqlConnection.QueryFirstOrDefaultAsync<int>(query, args);

            return id;


        }

        public async Task<PessoaDto> GetPessoaByIdConsulta(int idConsulta)
        {
            var query = @"SELECT P.Id
                              ,Nome
                              ,Sobrenome
                              ,Endereco
                              ,Numero
                              ,Bairro
                              ,Complemento
                              ,CEP
                              ,CPF
                              ,RG
                              ,Telefone
                              ,DataNascimento
                          FROM Tb_Pessoa P
						  join dbo.Tb_Consulta C on C.PessoaId = P.Id
						  Where C.Id = @idConsulta";

            var result = await Db.SqlConnection.QueryFirstOrDefaultAsync<PessoaDto>(query, new { idConsulta });

            return result;
        }

        public Task<PessoaDto> GetPessoaByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }

}
