using Futebol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Futebol.Controllers
{
    public class HomeController : Controller
    {
        private LigaTabajaraContext db = new LigaTabajaraContext();

        public ActionResult Index()
        {
            var times = db.Times.Include(t => t.Jogadores).Include(t => t.ComissaoTecnica).ToList();

            bool ligaApta = VerificarLigaApta(times);

            ViewBag.LigaApta = ligaApta;
            return View(times);
        }

        private bool VerificarLigaApta(List<Time> times)
        {
            if (times.Count != 20)
                return false;

            foreach (var time in times)
            {
                if (time.Jogadores == null || time.Jogadores.Count < 30)
                    return false;

                if (time.ComissaoTecnica == null || time.ComissaoTecnica.Count < 5)
                    return false;

                var cargosDistintos = time.ComissaoTecnica.Select(c => c.Cargo).Distinct().Count();
                if (cargosDistintos < 5)
                    return false;
            }

            return true;
        }
    }
}