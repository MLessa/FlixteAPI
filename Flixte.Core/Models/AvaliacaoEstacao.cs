using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Models
{
    public class AvaliacaoEstacao
    {
        public int ID { get; set; }
        public int IDEstacao { get; set; }
        public int IDUsuario { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
