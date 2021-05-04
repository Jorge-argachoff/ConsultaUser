using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Dtos
{
   public class RegisterUserDto
    {
        [Required(ErrorMessage ="Campo obrigatorio.")]
        [EmailAddress(ErrorMessage ="Email invalido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo obrigatorio.")]
        [StringLength(100,ErrorMessage ="O campo deve conter 1 até 100 carateres.")]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="As senhas não conferem.")]
        public string ConfirmPassword { get; set; }

       public string Nome { get; set; }

       public string Sobrenome { get; set; }

       public string Cpf { get; set; }

       public string Celular { get; set; }

       public string DDD { get; set; }

        public string Role { get; set; }

        public DateTime DataNascimento { get; set; }
    }

    public class LoginUserDto
    {
        [Required(ErrorMessage = "Campo obrigatorio.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo obrigatorio.")]
        public string Password { get; set; }
    }

    public class RoleDto
    {
        [Required(ErrorMessage = "Campo obrigatorio.")]
        public string Name { get; set; }
      
    }
}
