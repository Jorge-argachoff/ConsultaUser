using Microsoft.AspNetCore.Identity;


namespace Domain.Entities
{
    public class User:IdentityUser
    {
        public int PessoaId { get; set; }        
    }
}
