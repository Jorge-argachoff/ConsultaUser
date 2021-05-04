using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Infra.Context;
using Infra.Dapper;
using Infra.Interfaces;
using Infra.Repositories;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaUserApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            //JWT
            var existe = Configuration.GetSection("AppTokenSettings").Exists();
            var appSettingsSection = Configuration.GetSection("AppTokenSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appsSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appsSettings.Secret);

            string mySqlConnection = Configuration.GetConnectionString("IdentityConnection");

            services.AddDbContext<AppDbContext>(options => options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));
            //IDENTITY
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddTransient(_ => new DapperDb(Configuration.GetConnectionString("SqlServerConnection")));
            


            //Application
            services.AddTransient<IPessoaService, PessoaService>();            
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITokenService, TokenService>();

            //Infra           
            services.AddTransient<IPessoaRepository, PessoaRepository>();


            services.AddCors();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ConsultaUserApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ConsultaUserApi v1"));
            }
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
