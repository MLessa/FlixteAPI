﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flixte.Core.Models
{
    public class EstacaoYChannel
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Nome")]
        public int IDEstacao { get; set; }
        public int IDYChannel { get; set; }
        public DateTime Data { get; set; }
    }
}
