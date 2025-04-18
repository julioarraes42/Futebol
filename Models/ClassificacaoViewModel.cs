using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Futebol.Models
{
    public class ClassificacaoViewModel
    {
        public int TimeId { get; set; }
        public string NomeTime { get; set; }
        public int Pontos { get; set; }
        public int Vitorias { get; set; }
        public int Empates { get; set; }
        public int Derrotas { get; set; }
        public int GolsMarcados { get; set; }
        public int GolsSofridos { get; set; }

        public int SaldoGols => GolsMarcados - GolsSofridos;

        public double Aproveitamento
        {
            get
            {
                int jogos = Vitorias + Empates + Derrotas;
                if (jogos == 0) return 0;
                return (double)(Pontos * 100) / (jogos * 3);
            }
        }
    }
}