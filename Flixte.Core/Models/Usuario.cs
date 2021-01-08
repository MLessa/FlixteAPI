using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Models
{

    public class APIResponseUser
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Title { get; set; }
    }

    public class Usuario
    {
        public int ID { get; set; }        
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Login { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }
        public bool Ativo { get; set; }
        public bool Admin { get; set; }
        public int IDTipoUsuario { get; set; }
        public DateTime Data { get; set; }
        public DateTime UltimoAcesso { get; set; }
        public string FBEmailAddress { get; set; }
        public string FBAccountID { get; set; }
        public string FBToken { get; set; }
        public string GGEmailAddress { get; set; }
        public string GGAccountID { get; set; }
        public string GGToken { get; set; }
        public string Title { get { return string.IsNullOrEmpty(Nome) ? "--" : (Nome.Length > 2 ? Nome.Substring(0, 2).ToUpper() : Nome.ToUpper()); } }
        public string UserName { get; set; }
        public string ImageURL { get; set; }

        public APIResponseUser APIResponseUser
        {
            get
            {
                return new APIResponseUser
                {
                    Name = this.Nome,
                    Login = this.Login,
                    Title = this.Title
                };
            }
            private set { }
        }
    }
}
