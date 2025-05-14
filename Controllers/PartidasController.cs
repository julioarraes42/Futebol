using Futebol.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Futebol.Controllers
{
    public class PartidasController : Controller
    {
        private LigaTabajaraContext db = new LigaTabajaraContext();

        // GET: Partidas
        public ActionResult Index()
        {
            var partidas = db.Partidas.Include(p => p.TimeCasa).Include(p => p.TimeFora);
            return View(partidas.ToList());
        }

        // GET: Partidas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Partida partida = db.Partidas.Include(p => p.TimeCasa).Include(p => p.TimeFora).FirstOrDefault(p => p.Id == id);

            if (partida == null)
                return HttpNotFound();

            return View(partida);
        }

        // GET: Partidas/Create
        public ActionResult Create()
        {
            ViewBag.TimeCasaId = new SelectList(db.Times, "Id", "Nome");
            ViewBag.TimeForaId = new SelectList(db.Times, "Id", "Nome");
            return View();
        }

        // POST: Partidas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DataHora,Local,TimeCasaId,TimeForaId,PlacarCasa,PlacarFora")] Partida partida)
        {
            if (ModelState.IsValid)
            {
                db.Partidas.Add(partida);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TimeCasaId = new SelectList(db.Times, "Id", "Nome", partida.TimeCasaId);
            ViewBag.TimeForaId = new SelectList(db.Times, "Id", "Nome", partida.TimeForaId);
            return View(partida);
        }

        // GET: Partidas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Partida partida = db.Partidas.Find(id);
            if (partida == null)
                return HttpNotFound();

            ViewBag.TimeCasaId = new SelectList(db.Times, "Id", "Nome", partida.TimeCasaId);
            ViewBag.TimeForaId = new SelectList(db.Times, "Id", "Nome", partida.TimeForaId);
            return View(partida);
        }

        // POST: Partidas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DataHora,Local,TimeCasaId,TimeForaId,PlacarCasa,PlacarFora")] Partida partida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TimeCasaId = new SelectList(db.Times, "Id", "Nome", partida.TimeCasaId);
            ViewBag.TimeForaId = new SelectList(db.Times, "Id", "Nome", partida.TimeForaId);
            return View(partida);
        }

        // GET: Partidas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Partida partida = db.Partidas.Find(id);
            if (partida == null)
                return HttpNotFound();

            return View(partida);
        }

        // POST: Partidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Partida partida = db.Partidas.Find(id);
            db.Partidas.Remove(partida);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GerarPartidas()
        {
            var times = db.Times.Where(t => t.AptoParaLiga).ToList();

            if (times.Count != 20)
            {
                TempData["Erro"] = "É necessário exatamente 20 times aptos para gerar o campeonato.";
                return RedirectToAction("Index");
            }

            var partidas = new List<Partida>();
            int rodada = 1;

            for (int i = 0; i < times.Count; i++)
            {
                for (int j = 0; j < times.Count; j++)
                {
                    if (i != j)
                    {
                        partidas.Add(new Partida
                        {
                            TimeCasaId = times[i].Id,
                            TimeForaId = times[j].Id,
                            Data = DateTime.Now.AddDays(rodada),
                            Rodada = rodada
                        });

                        rodada++;
                    }
                }
            }

            db.Partidas.AddRange(partidas);
            db.SaveChanges();

            TempData["Sucesso"] = "Partidas geradas com sucesso!";
            return RedirectToAction("Index");
        }

        public ActionResult RegistrarResultado(int id)
        {
            var partida = db.Partidas
                .Include(p => p.TimeCasa)
                .Include(p => p.TimeFora)
                .FirstOrDefault(p => p.Id == id);

            if (partida == null)
            {
                return HttpNotFound();
            }

            if (partida.GolsCasa == null && partida.GolsFora == null)
            {
                Random random = new Random();
                partida.GolsCasa = random.Next(0, 5);
                partida.GolsFora = random.Next(0, 5);
            }

            var jogadoresCasa = db.Jogadores.Where(j => j.TimeId == partida.TimeCasa.Id).ToList();
            var jogadoresFora = db.Jogadores.Where(j => j.TimeId == partida.TimeFora.Id).ToList();

            ViewBag.JogadoresCasa = new SelectList(jogadoresCasa, "Id", "Nome");
            ViewBag.JogadoresFora = new SelectList(jogadoresFora, "Id", "Nome");

            return View(partida);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarResultado(Partida partidaForm, List<int> GolsCasa, List<int> GolsFora)
        {
            var partida = db.Partidas
                .Include(p => p.TimeCasa)
                .Include(p => p.TimeFora)
                .FirstOrDefault(p => p.Id == partidaForm.Id);

            if (partida == null)
            {
                return HttpNotFound();
            }

            partida.GolsCasa = partidaForm.GolsCasa;
            partida.GolsFora = partidaForm.GolsFora;

            foreach (var jogadorId in GolsCasa)
            {
                var estatistica = new Estatistica
                {
                    PartidaId = partida.Id,
                    JogadorId = jogadorId,
                    Gols = 1
                };
                db.Estatisticas.Add(estatistica);
            }

            foreach (var jogadorId in GolsFora)
            {
                var estatistica = new Estatistica
                {
                    PartidaId = partida.Id,
                    JogadorId = jogadorId,
                    Gols = 1
                };
                db.Estatisticas.Add(estatistica);
            }

            db.SaveChanges();

            return RedirectToAction("Index"); 
        }
    }
}