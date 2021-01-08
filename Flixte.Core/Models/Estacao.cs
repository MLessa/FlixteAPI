using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Models
{
    public class Estacao
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        public int IDUsuario { get; set; }
        public string Descricao { get; set; }
        public string Tags { get; set; }
        public string URLLogo { get; set; }
        public bool Ativo { get; set; }
        public bool Destaque { get; set; }
        public DateTime Data { get; set; }
        public int Users { get; set; }
        public int Views { get; set; }
        public bool Excluido { get; set; }
        public bool Privado { get; set; }
        public string Senha { get; set; }
   
        
    }

    public class EstacaoRate
    {
        public int ID { get; set; }
        public int Rate { get; set; }
    }
}
