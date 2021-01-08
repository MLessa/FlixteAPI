using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Models
{
    public class Token
    {
        public int ID { get; set; }
        public string TokenValue { get; set; }
        public int IDUsuario { get; set; }
        public bool Ativo { get; set; }
        public DateTime Data { get; set; }
        public string DeviceID { get; set; }
        public string App { get; set; }
        public DateTime UltimoAcesso { get; set; }
    }
}
