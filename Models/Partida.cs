using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Futebol.Models
{
    public class Partida
    {
        public int Id { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        public int Rodada { get; set; }

        public int TimeCasaId { get; set; }
        public virtual Time TimeCasa { get; set; }

        public int TimeForaId { get; set; }
        public virtual Time TimeFora { get; set; }

        public int? GolsCasa { get; set; }
        public int? GolsFora { get; set; }

        public virtual ICollection<Estatistica> Estatisticas { get; set; }
    }
}