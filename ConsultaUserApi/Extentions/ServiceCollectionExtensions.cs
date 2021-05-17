using Application.Interfaces;
using Application.Services;
using Infra.Interfaces;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultaUserApi.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfraInjections(this IServiceCollection services)
        {
            services.AddTransient<IPessoaRepository, PessoaRepository>();


            return services;
        }

        public static IServiceCollection AddApplicationInjections(this IServiceCollection services)
        {

            services.AddTransient<IPessoaService, PessoaService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITokenService, TokenService>();


            return services;
        }
    }
}
