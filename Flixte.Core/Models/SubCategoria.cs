using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Models
{
    public class SubCategoria
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        public int IDCategoria { get; set; }
        public int IDGrupo { get; set; }
    }
}
