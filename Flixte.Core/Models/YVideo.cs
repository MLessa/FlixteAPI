using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Models
{
    public class YVideo
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        public string YChannelID { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public string IDExterno { get; set; }
        public string URL { get; set; }
        public string URLLogo { get; set; }
        public bool Ativo { get; set; }        
    }
}
