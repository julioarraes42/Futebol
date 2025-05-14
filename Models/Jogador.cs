using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Futebol.Models
{
    public enum Posicao
    {
        Goleiro,
        Zagueiro,
        Volante,
        Meia,
        Atacante
    }

    public enum PePreferido
    {
        Direito,
        Esquerdo,
        Ambidestro
    }

    public class Jogador
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }
        public string Nacionalidade { get; set; }
        public Posicao Posicao { get; set; }
        public int NumeroCamisa { get; set; }
        public float Altura { get; set; }
        public float Peso { get; set; }
        public PePreferido PePreferido { get; set; }

        [Required] 
        public int TimeId { get; set; }

        [Required] 
        public virtual Time Time { get; set; }
    }
}