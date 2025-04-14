using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Futebol.Models
{
    public class Time
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public int AnoFundacao { get; set; }
        public string Estadio { get; set; }
        public int CapacidadeEstadio { get; set; }
        public string CorUniformePrimaria { get; set; }
        public string CorUniformeSecundaria { get; set; }
        public bool AptoParaLiga { get; set; }
        public virtual ICollection<Jogador> Jogadores { get; set; }
        public virtual ICollection<ComissaoTecnica> ComissaoTecnica { get; set; }
    }
}