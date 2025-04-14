using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Futebol.Models
{
    public enum Cargo
    {
        Treinador,
        Auxiliar,
        PreparadorFisico,
        Fisiologista,
        TreinadorGoleiros,
        Fisioterapeuta
    }

    public class ComissaoTecnica
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Cargo Cargo { get; set; }
        public DateTime DataNascimento { get; set; }

        public int TimeId { get; set; }
        public virtual Time Time { get; set; }
    }
}