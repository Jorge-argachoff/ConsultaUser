using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestConsultaUserApi
{
    public class UserTestes
    {


        public UserTestes()
        {

        }

        [Fact]
        public void AddUser_Sucesso()
        {

            var userServiceMoq = new Mock<IUserService>();

            var result = userServiceMoq.Setup(s => s.AddUser(new Domain.Dtos.RegisterUserDto { Email = "teste@teste.com", Password = "Teste", Role = "" })).ReturnsAsync(IdentityResult.Success);
                        
            Assert.NotNull(result);           

        }
    }
}
