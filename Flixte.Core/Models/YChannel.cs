using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Models
{
    public class YChannel
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public string Refreshed { get; set; }
        public string URLLogo { get; set; }
        public string IDExterno { get; set; }
        public int IDCategoria { get; set; }
        public int IDSubCategoria { get; set; }        
    }
}
