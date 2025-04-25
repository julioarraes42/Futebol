using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }

        [Required] 
        public int TimeId { get; set; }

        [Required] 
        public virtual Time Time { get; set; }
    }
}